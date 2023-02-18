using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IObserver
{
    [SerializeField] GameObject hover_ui;
    [SerializeField] GameObject selected_ui;
    [SerializeField] Unit unit_reference;
    // Start is called before the first frame update
    void Start()
    {
        unit_reference = this.gameObject.GetComponent<Unit>();
        //Debug.Log("unit_reference: " + unit_reference);
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
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
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
}
