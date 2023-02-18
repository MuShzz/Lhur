using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Unit : Subject
{
    [SerializeField] SkillSO weaponSO;

    public PlayerS player_reference;
    public Skill weapon;
    public List<Skill> skills;

    public int unit_seed;
    public string unit_name;
    public string unit_sex;
    public FactionName unit_faction;

    public Row      row;
    public Column   column;

    void Start()
    {
        weapon = gameObject.AddComponent(typeof(Skill)) as Skill;
        weapon.skillSO = weaponSO;
        skills.Add(this.weapon);
        skills.Add(this.weapon);
        skills.Add(this.weapon);
        skills.Add(this.weapon);
        skills.Add(this.weapon);
    }
    public void Init(int unit_seed, string unit_name, string unit_sex, FactionName unit_faction, Skill weapon)
    {
        this.unit_seed      = unit_seed;
        this.unit_name      = unit_name;
        this.unit_sex       = unit_sex;
        this.unit_faction   = unit_faction;
        this.weapon         = weapon;
        skills              = new List<Skill>();
        skills.Add(this.weapon);
    }
    public void DebugDetails()
    {
        Debug.Log("Name: " + this.unit_name + " - " + this.unit_sex + " - " + this.unit_faction + " - " + this.weapon.skillSO.skillName + " - SEED: " + unit_seed);
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
        if (this.player_reference.selectedUnit == this)
        {
            if (IsOwner) this.player_reference.SetSelectedUnit(null);

            this.NotifyObservers(NotifyAction.Deselect);
        }
        else
        {
            if (IsOwner) this.player_reference.SetSelectedUnit(this);

            this.NotifyObservers(NotifyAction.Select);
        }
    }
    #endregion Network

    public void UnitClicked()
    {
        if(IsOwner) SubmitSelectUnitServerRpc();

        //Local only behaviour
        if (this.player_reference.selectedUnit != this)
        {
            Debug.Log("Refreshing MenuUI with NULL");
            this.player_reference.RefreshMenuUI(null);
        }
        else
        {
            Debug.Log("Refreshing MenuUI with skills "+ skills.Count);
            this.player_reference.RefreshMenuUI(skills);
        }
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
