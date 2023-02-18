using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject hover_ui;
    public GameObject selected_ui;
    public PlayerS player_reference;
    public Skill skill_reference;
    public Boolean selected = false;
    public GameObject skillImage;

    // Start is called before the first frame update
    void Start()
    {
        if(selected) selected_ui.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        player_reference.SkillClicked(skill_reference, this);
    }
    public void SelectSkill()
    {
        selected_ui.SetActive(true);
        if(!selected)
        {
            RectTransform skillRect = gameObject.GetComponent<RectTransform>();
            skillRect.sizeDelta = new Vector2(skillRect.sizeDelta.x + 50, skillRect.sizeDelta.y + 50);
        }
        selected = true;
    }

    public void UnSelectSkill()
    {
        selected_ui.SetActive(false);
        
        if (selected)
        {
            RectTransform skillRect = gameObject.GetComponent<RectTransform>();
            skillRect.sizeDelta = new Vector2(skillRect.sizeDelta.x - 50, skillRect.sizeDelta.y - 50);
        }
        selected = false;
    }
    public void OnNotifyParams(NotifyAction notifyAction, Dictionary<string, object> notifyParams)
    {
        return;
    }
}
