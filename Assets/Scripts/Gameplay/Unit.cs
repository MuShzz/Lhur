using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Unit : Subject
{
    public PlayerS player;
    public Weapon weapon;

    public int unit_seed;
    public string unit_name;
    public string unit_sex;
    public FactionName unit_faction;

    public Row row;
    public Column column;

    public void Init(int unit_seed, string unit_name, string unit_sex, FactionName unit_faction, Weapon weapon)
    {
        this.unit_seed = unit_seed;
        this.unit_name = unit_name;
        this.unit_sex = unit_sex;
        this.unit_faction = unit_faction;
        this.weapon = weapon;
    }
    public void DebugDetails()
    {
        Debug.Log("Name: " + this.unit_name + " - " + this.unit_sex + " - " + this.unit_faction + " - " + this.weapon.weapon_name + " - SEED: " + unit_seed);
    }
    #region Network
    [ServerRpc]
    void SubmitSelectUnitServerRpc()
    {
        SubmitSelectUnitClientRpc();
    }
    [ClientRpc]
    void SubmitSelectUnitClientRpc()
    {
        if (this.player.selectedUnit == this)
        {
            if (IsOwner) this.player.SetSelectedUnit(null);

            this.NotifyObservers(NotifyAction.Deselect);
        }
        else
        {
            if (IsOwner) this.player.SetSelectedUnit(this);

            this.NotifyObservers(NotifyAction.Select);
        }
    }
    #endregion Network

    public void UnitClicked()
    {
        if(IsOwner) SubmitSelectUnitServerRpc();
    }
    public void PlayerDeselectUnit() { this.NotifyObservers(NotifyAction.Deselect); }
    public void Hover() {
        //this.NotifyObservers(NotifyAction.Hover);
    }
    public Boolean comparePosition(Unit toCompare)
    {
        if(toCompare.row == row && toCompare.column == column)
        {
            return true;
        }
        return false;
    }
    
}
