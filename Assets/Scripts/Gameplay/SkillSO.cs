using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public SkillType skillType;
    public Range range;
    public Aim aim;
    public Splash splash;
    public DamageType damageType;
    public int base_damage;
    public int baseCooldown = 1;

    //UI Section
    public Sprite menuIcon;
    public GameObject visualEffect;

    public SkillSO(SkillSO existingSkill)
    {
        this.skillName = existingSkill.skillName;
        this.range = existingSkill.range;
        this.aim = existingSkill.aim;
        this.splash = existingSkill.splash;
        this.damageType = existingSkill.damageType;
        this.base_damage = existingSkill.base_damage;
    }
    public void Print()
    {
        Debug.Log(skillName + " " +
                 skillType + " " +
                 range.ToString() + " " +
                 aim.ToString() + " " +
                 splash.ToString() + " " +
                 damageType.ToString() + " " +
                 base_damage);
    }

}
