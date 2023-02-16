using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public WeaponName weapon_name;
    public Range range;
    public Aim aim;
    public Splash splash;
    public DamageType damageType;
    public int base_damage;

    //UI Section
    public Sprite selectionMenuIcon;
    
    public Weapon(Weapon existingWeapon)
    {
        this.weapon_name    = existingWeapon.weapon_name;
        this.range          = existingWeapon.range;
        this.aim            = existingWeapon.aim;
        this.splash         = existingWeapon.splash;
        this.damageType     = existingWeapon.damageType;
        this.base_damage    = existingWeapon.base_damage;
    }
    public void Print() {
        Debug.Log(weapon_name + " " + 
                  range.ToString() + " " + 
                  aim.ToString() + " " + 
                  splash.ToString() + " " + 
                  damageType.ToString() + " " +
                  base_damage);
    }
}
