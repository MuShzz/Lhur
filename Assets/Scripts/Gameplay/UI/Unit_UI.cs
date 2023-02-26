using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IObserver
{
    [SerializeField] public GameObject hover_ui;
    [SerializeField] GameObject selected_ui;
    [SerializeField] Unit unit_reference;
    [SerializeField] public Player_UI player_UIReference;
    [SerializeField] public int row;
    [SerializeField] public int column;

    public List<Unit_UI> rowUnits       = new List<Unit_UI>();
    public List<Unit_UI> columnUnits    = new List<Unit_UI>();
    public List<Unit_UI> adjacentUnits  = new List<Unit_UI>();
    public List<Unit_UI> frontUnits     = new List<Unit_UI>();

    // Start is called before the first frame update
    void Start()
    {
        unit_reference = this.gameObject.GetComponent<Unit>();
        MapReferences();
    }
    private void MapReferences()
    {
        foreach(Unit_UI unitUIRef in player_UIReference.ui_units)
        {
            if(unitUIRef.row == row)
            {
                rowUnits.Add(unitUIRef);
                if(unitUIRef.column == column+1 || unitUIRef.column == column - 1)
                {
                    adjacentUnits.Add(unitUIRef);
                    Debug.Log("Unit_UI MapReferences | adjacent "+row+" "+column+" --- "+ unitUIRef.row+" "+ unitUIRef.column);
                }
            }
            if (unitUIRef.column == column)
            {
                columnUnits.Add(unitUIRef);
                if (unitUIRef.row == row + 1 || unitUIRef.row == row - 1)
                {
                    adjacentUnits.Add(unitUIRef);
                    Debug.Log("Unit_UI MapReferences | adjacent " + row + " " + column + " --- " + unitUIRef.row + " " + unitUIRef.column);
                }
            }
        }
    }
    private void OnEnable()
    {
        unit_reference.AddObserver(this);
    }
    private void OnDisable()
    {
        unit_reference.RemoveObserver(this);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        unit_reference.UnitHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        unit_reference.UnitHoverExit();
    }

    public void OnNotify(NotifyAction notifyAction)
    {
        switch (notifyAction)
        {
            case NotifyAction.HoverIn:  hover_ui.SetActive(true); break;
            case NotifyAction.HoverOut: hover_ui.SetActive(false); break;
            case NotifyAction.Select:   selected_ui.SetActive(true); break;
            case NotifyAction.Deselect: selected_ui.SetActive(false); break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Clicked this unit! " + unit_reference.row + " - " + unit_reference.column);
        unit_reference.UnitClicked();
        
    }

    public void OnNotifyParams(NotifyAction notifyAction, Dictionary<string, object> notifyParams)
    {
        return;
    }

    public List<Unit_UI> TargetUnitsUI(Aim aim, Splash splash) 
    {
        List<Unit_UI> unitsUI = new List<Unit_UI>();
        switch (aim)
        {
            case Aim.Single:
                hover_ui.SetActive(true);
                break;
            case Aim.All:
                foreach (Unit_UI unitUIRef in player_UIReference.ui_units)
                {
                    unitsUI.Add(unitUIRef);
                }
                break;
            case Aim.Column:
                foreach (Unit_UI unitUIRef in columnUnits)
                {
                    if (splash == Splash.All || this == unitUIRef) { unitsUI.Add(unitUIRef); }
                    else if(splash == Splash.Adjacent && adjacentUnits.Contains(unitUIRef)) {
                        unitsUI.Add(unitUIRef);
                    }
                    
                }
                break;
            case Aim.Row:
                foreach (Unit_UI unitUIRef in rowUnits)
                {
                    if (splash == Splash.All || this == unitUIRef) { unitsUI.Add(unitUIRef); }
                    else if (splash == Splash.Adjacent && adjacentUnits.Contains(unitUIRef))
                    {
                        unitsUI.Add(unitUIRef);
                    }
                }
                break;
            case Aim.RowColumn:
                foreach (Unit_UI unitUIRef in columnUnits)
                {
                    if (splash == Splash.All || this == unitUIRef) { unitsUI.Add(unitUIRef); }
                    else if (splash == Splash.Adjacent && adjacentUnits.Contains(unitUIRef))
                    {
                        unitsUI.Add(unitUIRef);
                    }
                }
                foreach (Unit_UI unitUIRef in rowUnits)
                {
                    if (splash == Splash.All || this == unitUIRef) { unitsUI.Add(unitUIRef); }
                    else if (splash == Splash.Adjacent && adjacentUnits.Contains(unitUIRef))
                    {
                        unitsUI.Add(unitUIRef);
                    }
                }
                break;
        }
        return unitsUI;
    }
}
