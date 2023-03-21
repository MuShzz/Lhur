using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerS : Subject
{
    [SerializeField] GameObject playerLeftTemplate;
    [SerializeField] GameObject playerRightTemplate;
    
    [SerializeField] RectTransform playerParentTranform;
    [SerializeField] RectTransform playerInfo;
    [SerializeField] List<Skill> playerSkills = new List<Skill>();
    [SerializeField] public Player_UI playerUI;

    public NetworkVariable<int> playerNumber;
    public Unit selectedUnit = null;
    public Skill selectedSkill = null;

    private PlayerPosition playerPos;
    void Start()
    {
        this.GetComponent<Canvas>().worldCamera= Camera.main;
        SetPlayerPositions();
        SetPlayersOnTurnManager();
        GameTurnManager.instance.SpawnPlayer(this);
        Debug.Log("PlayerS Start | playerNumber.Value: " + playerNumber.Value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetPlayersOnTurnManager()
    {
        if (IsOwner) { GameTurnManager.instance.myPlayer = this; }
        else { GameTurnManager.instance.enemyPlayer = this; }
    }
    public override void OnNetworkSpawn()
    {
        //GameTurnManager.instance.SpawnPlayer(this);
    }
    public void SetSelectedUnit(Unit selectedUnit)
    {
        if(this.selectedUnit != null)
        {
            this.selectedUnit.PlayerDeselectUnit();
        }
        this.selectedUnit = selectedUnit;
    }

    public Unit GetUnit(Column column, Row row) 
    {
        foreach (Unit_UI unit_ui in playerUI.ui_units)
        {
            Unit unit = unit_ui.gameObject.GetComponent<Unit>();
            if (unit.column == column && unit.row == row)
            {
                return unit;
            }
        }
        return null;
    }
    #region Skills
    public void SkillClicked(Skill skill_reference, Skill_UI skillUI)
    {
        Debug.Log("PlayerS SkillClicked | " + IsOwner+" "+ skill_reference);
        if (IsOwner) {
            selectedSkill = skill_reference; 
        }

        Dictionary<string, object> notifyParams = new Dictionary<string, object>();
        notifyParams.Add("skillUI", skillUI);
        NotifyObservers(NotifyAction.SkillSelect, notifyParams);
    }
    public void RefreshMenuUI(List<Skill> skills){
        Dictionary<string,object> notifyParams = new Dictionary<string, object>();
        //Debug.Log("skills Count"+ skills.Count);
        if (skills == null) { skills = this.playerSkills; }

        Debug.Log("PlayerS RefreshMenuUI | skillsCount"+ skills.Count);
        notifyParams.Add("skills",skills);

        object skillsObj = notifyParams["skills"];
        List<Skill> skillsT = (List<Skill>)skillsObj;
        Debug.Log("PlayerS RefreshMenuUI | skillsT - " + skillsT.Count);
        NotifyObservers(NotifyAction.Select, notifyParams);
    }
    #endregion
    private void SetPlayerPositions()
    {
        Unit[] playerUnits;
        RectTransform t_playerRectTransform; Unit[] t_playerUnits; RectTransform t_playerInfo;
        Debug.Log("IsOwner: " + IsOwner);
        if (IsOwner)
        {
            t_playerRectTransform = playerLeftTemplate.GetComponent<RectTransform>();
            t_playerUnits = playerLeftTemplate.GetComponentsInChildren<Unit>();
            t_playerInfo = playerLeftTemplate.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            playerPos = PlayerPosition.Left;

        }
        else
        {
            t_playerRectTransform = playerRightTemplate.GetComponent<RectTransform>();
            t_playerUnits = playerRightTemplate.GetComponentsInChildren<Unit>();
            t_playerInfo = playerRightTemplate.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
            playerPos = PlayerPosition.Right;
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

                if (playerPos == PlayerPosition.Left) break;
                unitRT.localScale = new Vector3(-unitRT.localScale.x, unitRT.localScale.y, unitRT.localScale.z);
                break;

            }
        }
    }
}
