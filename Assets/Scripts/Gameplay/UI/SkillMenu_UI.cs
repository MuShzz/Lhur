using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class SkillMenu_UI : MonoBehaviour, IObserver
{
    [SerializeField] PlayerS playerReference;
    [SerializeField] Skill_UI[] skillUIs;
    void Start()
    {
        if (!playerReference.IsOwner){
            Destroy(gameObject);
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable() {
        playerReference.AddObserver(this);
    }
    private void OnDisable()
    {
        playerReference.RemoveObserver(this);
    }
    public void OnNotify(NotifyAction notifyAction)
    {
    }
    public void OnNotifyParams(NotifyAction notifyAction, Dictionary<string, object> notifyParams){
        //Debug.Log("SkillMenu_UI OnNotifyParams notifyParams.Count - " + notifyParams.Count);
        switch (notifyAction){
            case NotifyAction.Select:
                List<Skill> skills = (List<Skill>)notifyParams["skills"];
                Debug.Log("SkillMenu_UI.OnNotifyParams | OnNotifyParams skills.Count - " + skills.Count);
                for (int i = 0; i < skills.Count; i++){
                    Skill skill = skills[i];
                    Debug.Log(skill);
                    if(skill != null){
                        Debug.Log("SkillMenu_UI.OnNotifyParams | Skill is not null");
                        skillUIs[i].gameObject.SetActive(true);
                        skillUIs[i].skill_reference = skill;
                        skillUIs[i].skillImage.gameObject.GetComponent<Image>().sprite = skill.skillSO.menuIcon;
                        if (i == 0) { skillUIs[i].SelectSkill(); }
                        else { skillUIs[i].UnSelectSkill(); }
                    }
                }
                for (int i = skills.Count; i < skillUIs.Length; i++){
                    skillUIs[i].gameObject.SetActive(false);
                }
                break;
            case NotifyAction.SkillSelect:
                
                Skill_UI skillUI = (Skill_UI)notifyParams["skillUI"];
                //if (skillUI.gameObject.GetComponent<Skill>().IsOwner == false) { return; }

                Debug.Log("SkillMenu_UI.OnNotifyParams | skillUI.name: " + skillUI.name);
                foreach(Skill_UI skillLoop in skillUIs) {
                    Debug.Log("skillLoop.name: " + skillLoop.name);
                    if (skillLoop.name == skillUI.name)
                    {
                        Debug.Log("SkillMenu_UI.OnNotifyParams | Equals!!!!");
                        skillLoop.SelectSkill();
                    }
                    else
                    {
                        skillLoop.UnSelectSkill();
                    }
                }
                break;
        }
    }

    public void UnSelectAll()
    {
        foreach(Skill_UI su in skillUIs){ su.UnSelectSkill(); }
    }
    // Update is called once per frame
    void Update()
    {
        ListKeyboard();
    }
    #region Keyboard
    private void ListKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSkillPos(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSkillPos(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSkillPos(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSkillPos(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSkillPos(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSkillPos(5);
        }
    }
    private void SelectSkillPos(int index) {
        if(skillUIs[index].skill_reference != null)
        {
            skillUIs[index].SkillUIClicked();
        }
    }
    #endregion
}
