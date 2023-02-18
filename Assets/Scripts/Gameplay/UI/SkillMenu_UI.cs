using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class SkillMenu_UI : MonoBehaviour, IObserver
{
    [SerializeField] PlayerS playerReference;
    [SerializeField] Skill_UI[] skillUIs;

    private void OnEnable()
    {
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
                Debug.Log("SkillMenu_UI OnNotifyParams skills.Count - " + skills.Count);
                for (int i = 0; i < skills.Count; i++){
                    Skill skill = skills[i];
                    Debug.Log(skill);
                    if(skill != null){
                        Debug.Log("Skill is not null");
                        skillUIs[i].gameObject.SetActive(true);
                        skillUIs[i].skillImage.gameObject.GetComponent<Image>().sprite = skill.skillSO.menuIcon;
                        if (i == 0) { skillUIs[i].SelectSkill(); }
                        else{ skillUIs[i].UnSelectSkill(); }
                    }
                }
                for (int i = skills.Count; i < skillUIs.Length; i++){
                    skillUIs[i].gameObject.SetActive(false);
                }
                break;
            case NotifyAction.SkillSelect:
                Skill_UI skillUI = (Skill_UI)notifyParams["skillUI"];
                Debug.Log("skillUI.name: "+ skillUI.name);
                foreach(Skill_UI skillLoop in skillUIs) {
                    Debug.Log("skillLoop.name: " + skillLoop.name);
                    if (skillLoop.name == skillUI.name)
                    {
                        Debug.Log("Equals!!!!");
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
