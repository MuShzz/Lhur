using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSkillCommand : ICommand
{
    int casterPlayer; 
    Column casterColumn; 
    Row casterRow; 
    int skillIndex; 
    int targetPlayer;
    Column targetColumn;
    Row targetRow;

    public CommandStatus commandStatus;

    public CastSkillCommand(int casterPlayer, Column casterColumn, Row casterRow, int skillIndex, int targetPlayer, Column targetColumn, Row targetRow)
    {
        this.casterPlayer   = casterPlayer;
        this.casterColumn   = casterColumn;
        this.casterRow      = casterRow;
        this.skillIndex     = skillIndex;
        this.targetPlayer   = targetPlayer;
        this.targetColumn   = targetColumn;
        this.targetRow      = targetRow;
        this.commandStatus  = CommandStatus.Pending;
    }
    public void execute()
    {
        commandStatus = CommandStatus.Executing;

        Debug.Log("CastSkillCommand - Executing - "+casterPlayer+" "+casterColumn + " " + casterRow + " " + skillIndex + " " + targetPlayer + " " + targetColumn + " " + targetRow);

        PlayerS caster = GameTurnManager.instance.GetPlayer(casterPlayer);
        PlayerS target = GameTurnManager.instance.GetPlayer(targetPlayer);

        Unit casterUnit = caster.GetUnit(casterColumn, casterRow);
        Unit targetUnit = target.GetUnit(targetColumn, targetRow);

        Skill skill = casterUnit.skills[skillIndex];
        Debug.Log("CastSkillCommand execute | casterUnit: " + casterUnit + " targetUnit: " + targetUnit + " skill: " + skill);

        List<Unit_UI> unitsUI = targetUnit.Unit_UIReference.TargetUnitsUI(skill.skillSO.aim, skill.skillSO.splash);
        foreach (Unit_UI unitUI in unitsUI)
        {
            skill.visualEffect(unitUI.unit_reference);
        }

        

        commandStatus = CommandStatus.Finished;
    }

    public CommandStatus getCommandStatus()
    {
        return commandStatus;
    }
}
