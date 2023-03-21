using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Unit : Subject
{
    [SerializeField] SkillSO weaponSO;

    public PlayerS playerReference;
    public Skill weapon;
    public List<Skill> skills;
    public Unit_UI Unit_UIReference;

    public int unit_seed;
    public string unit_name;
    public string unit_sex;
    public FactionName unit_faction;

    public Row      row;
    public Column   column;

    void Start()
    {
        Unit_UIReference = this.transform.GetComponent<Unit_UI>();
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
        if (this.playerReference.selectedUnit == this)
        {
            this.playerReference.SetSelectedUnit(null);
            this.NotifyObservers(NotifyAction.Deselect);
        }
        else
        {
            this.playerReference.SetSelectedUnit(this);
            this.NotifyObservers(NotifyAction.Select);
        }

        if (!IsOwner) { return; }

        //Local only behaviour
        if (this.playerReference.selectedUnit != this)
        {
            Debug.Log("Unit.UnitClicked | Refreshing MenuUI with NULL");
            this.playerReference.RefreshMenuUI(null);
        }
        else
        {
            Debug.Log("Unit.UnitClicked | Refreshing MenuUI with skills " + skills.Count);
            this.playerReference.RefreshMenuUI(skills);
        }
    }
 
    [ServerRpc(RequireOwnership = false)]
    void SubmitUnitHoverServerRPC(Boolean enter, Aim aim, Splash splash)
    {
        SubmitUnitHoverClientRPC(enter, aim, splash);
    }
    [ClientRpc]
    void SubmitUnitHoverClientRPC(Boolean enter, Aim aim, Splash splash)
    {
        Debug.Log("Unit SubmitUnitHoverClientRPC | "+enter+" "+aim+" "+splash);
        Unit_UI unitUI = this.gameObject.GetComponent<Unit_UI>();
        if (enter)
        {
            unitUI.player_UIReference.ShowTargets(unitUI, aim, splash);
        }
        else
        {
            unitUI.player_UIReference.HideTargets();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SubmitCastSkillServerRpc(int casterPlayer, Column casterColumn, Row casterRow, int skillIndex, int targetPlayer, Column targetColumn, Row targetRow)
    {
        Debug.Log("Unit SubmitCastSkillServerRpc | Starting)");
        if (skillIndex < 0) { Debug.Log("Unit SubmitCastSkillServerRpc | skillIndex: " + skillIndex);  return; }

        SubmitCastSkillClientRpc(casterPlayer, casterColumn, casterRow, skillIndex, targetPlayer, targetColumn, targetRow);
    }
    [ClientRpc]
    void SubmitCastSkillClientRpc(int casterPlayer, Column casterColumn, Row casterRow, int skillIndex, int targetPlayer, Column targetColumn, Row targetRow)
    {
        Debug.Log("Unit SubmitCastSkillClientRpc | Starting");
        CastSkillCommand csc = new CastSkillCommand(casterPlayer, casterColumn, casterRow, skillIndex, targetPlayer, targetColumn, targetRow);
        SkillInvoker.instance.AddQueue(csc);
        //csc.execute();
    }
    #endregion Network

    public void UnitClicked()
    {
        PlayerS myP = GameTurnManager.instance.myPlayer;

        if (myP.selectedUnit == null || myP.selectedSkill == null)
        {
            if (IsOwner){ SubmitSelectUnitServerRpc(); }
        }
        else
        {
            Debug.Log("Unit UnitClicked | GameTurnManager.instance.playerTurn: " + GameTurnManager.instance.playerTurn.Value);
            Debug.Log("Unit UnitClicked | myP.playerNumber: " + myP.playerNumber.Value);
            if (GameTurnManager.instance.playerTurn.Value != myP.playerNumber.Value) { Debug.Log("Unit UnitClicked | Not player turn - Returning"); return; }

            int skillIndex = myP.selectedUnit.skills.IndexOf(myP.selectedSkill);
            Debug.Log("Unit UnitClicked | skillIndex: " + skillIndex);
            Debug.Log("Unit UnitClicked | playerReference.playerNumber.Value: " + playerReference.playerNumber.Value);
            SubmitCastSkillServerRpc(myP.playerNumber.Value, myP.selectedUnit.column, myP.selectedUnit.row, skillIndex,
                                     playerReference.playerNumber.Value, column, row);
        }
    }
    public void UnitHoverEnter()
    {
        PlayerS myPlayer = GameTurnManager.instance.myPlayer;
        Debug.Log("Unit UnitHoverEnter | ENTERING HOVER");
        if(myPlayer.selectedUnit == null){
            Debug.Log("Unit UnitHoverEnter | ENTERING HOVER - myPlayer.selectedUnit NULL");
            return;
        }
        if (myPlayer.selectedSkill == null)
        {
            Debug.Log("Unit UnitHoverEnter | ENTERING HOVER - myPlayer.selectedSkill NULL");
            return;
        }
        if (!myPlayer.selectedUnit.skills.Contains(myPlayer.selectedSkill)){
            Debug.Log("Unit UnitHoverEnter | ENTERING HOVER - !myPlayer.selectedUnit.skills.Contains(myPlayer.selectedSkill)");
            return; 
        }
        if(column != Column.Front && myPlayer.selectedSkill.skillSO.range == Range.Melee)
        {
            Debug.Log("Unit UnitHoverEnter | ENTERING HOVER - Melee Skill)");
            return;
        }

        SubmitUnitHoverServerRPC(true, myPlayer.selectedSkill.skillSO.aim, myPlayer.selectedSkill.skillSO.splash);
    }
    public void UnitHoverExit()
    {
        Debug.Log("Unit UnitHoverExit | EXITING HOVER");
        SubmitUnitHoverServerRPC(false, Aim.Single, Splash.Single);
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
