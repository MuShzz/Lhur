using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UI : MonoBehaviour
{
    public List<Unit_UI> ui_units = new();
    public SkillMenu_UI skillMenuUI;
    public void ShowTargets(Unit_UI targetUnit, Aim aim, Splash splash)
    {
        Debug.Log("Player_UI ShowTargets | ");
        List<Unit_UI> unitsUI = targetUnit.TargetUnitsUI(aim, splash);
        foreach(Unit_UI unitUI in unitsUI)
        {
            unitUI.hover_ui.SetActive(true);
        }
    }
    public void HideTargets()
    {
        Debug.Log("Player_UI HideTargets | ");
        foreach (Unit_UI unitUI in ui_units)
        {
            unitUI.hover_ui.SetActive(false);
        }
    }
}
