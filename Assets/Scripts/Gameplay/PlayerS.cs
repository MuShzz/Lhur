using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerS : Subject
{
    [SerializeField] GameObject playerLeftTemplate;
    [SerializeField] GameObject playerRightTemplate;
    [SerializeField] RectTransform playerParentTranform;
    [SerializeField] RectTransform playerInfo;

    public Unit selectedUnit = null;

    private PlayerPosition playerPos;
    void Start()
    {
        Unit[] playerUnits;
        RectTransform t_playerRectTransform; Unit[] t_playerUnits; RectTransform t_playerInfo;
        Debug.Log("IsOwner: "+ IsOwner);
        if (IsOwner)
        {
            t_playerRectTransform   = playerLeftTemplate.GetComponent<RectTransform>();
            t_playerUnits           = playerLeftTemplate.GetComponentsInChildren<Unit>();
            t_playerInfo            = playerLeftTemplate.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            playerPos               = PlayerPosition.Left;

        }
        else
        {
            t_playerRectTransform   = playerRightTemplate.GetComponent<RectTransform>();
            t_playerUnits           = playerRightTemplate.GetComponentsInChildren<Unit>();
            t_playerInfo            = playerRightTemplate.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            playerPos               = PlayerPosition.Right;
        }

        //Player Position
        playerParentTranform.anchoredPosition = new Vector2(t_playerRectTransform.anchoredPosition.x, t_playerRectTransform.anchoredPosition.y);
        playerParentTranform.sizeDelta = new Vector2(t_playerRectTransform.sizeDelta.x, t_playerRectTransform.sizeDelta.y);
        
        //Player Info Position
        playerInfo.anchoredPosition = new Vector2(t_playerInfo.anchoredPosition.x, t_playerInfo.anchoredPosition.y);
        playerInfo.sizeDelta = new Vector2(t_playerInfo.sizeDelta.x, t_playerInfo.sizeDelta.y);
        
        //Player Units Position
        playerUnits = playerParentTranform.GetComponentsInChildren<Unit>();
        foreach (Unit unit in playerUnits)
        {
            foreach (Unit unitToCompare in t_playerUnits)
            {
                if (!unit.comparePosition(unitToCompare)) continue;
                
                RectTransform unitRT = unit.gameObject.GetComponent<RectTransform>();
                RectTransform unitToCompareRT = unitToCompare.gameObject.GetComponent<RectTransform>();
                unitRT.anchoredPosition = new Vector2(unitToCompareRT.anchoredPosition.x, unitToCompareRT.anchoredPosition.y);
                unitRT.sizeDelta = new Vector2(unitToCompareRT.sizeDelta.x, unitToCompareRT.sizeDelta.y);

                if(playerPos == PlayerPosition.Left) break;
                unitRT.localScale = new Vector3(-unitRT.localScale.x, unitRT.localScale.y, unitRT.localScale.z);
                break;
                
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSelectedUnit(Unit selectedUnit)
    {
        if(this.selectedUnit != null)
        {
            this.selectedUnit.PlayerDeselectUnit();
        }
        this.selectedUnit = selectedUnit;
    }
}
