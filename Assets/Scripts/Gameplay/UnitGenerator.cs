using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    List<FactionName> unit_factions = new List<FactionName>();
    List<string> vowels;
    List<string> body;

    Weapon weapon;

    string unit_name;
    string unit_sex;
    FactionName unit_faction;

    //Random.InitState((int) DateTime.Now.Ticks)

    // Start is called before the first frame update

    public GameObject unitPrefab;
    public GameObject unitPanel;
    public GameObject selectedUnitPrefab;
    public GameObject selectedUnitPanel;
    public List<Unit> selectedUnits;
    void Start()
    {
        unit_factions.AddRange((FactionName[])Enum.GetValues(typeof(FactionName)));
        vowels = new List<string>();
        vowels.Add("a"); vowels.Add("e"); vowels.Add("i"); vowels.Add("o"); vowels.Add("u");

        body = new List<string>();
        body.Add("b"); body.Add("c"); body.Add("d"); body.Add("f"); body.Add("g");
        body.Add("h"); body.Add("j"); body.Add("k"); body.Add("l"); body.Add("m");
        body.Add("n"); body.Add("p"); body.Add("q"); body.Add("r"); body.Add("s");
        body.Add("t"); body.Add("v"); body.Add("w"); body.Add("x"); body.Add("y");
        body.Add("z");

        //more common letters
        body.Add("b"); body.Add("c"); body.Add("d"); body.Add("f"); body.Add("g");

        
    }
    public Unit GenerateUnit(int unit_seed)
    {

        if (unit_seed < 0)
        {
            unit_seed = (int)UnityEngine.Random.Range(0, 1000000000);
        }

        UnityEngine.Random.InitState(unit_seed);

        generateName();
        generateWeapon();

        //Unit unit = (Unit)ScriptableObject.CreateInstance(typeof(Unit));
        //unit.Init(unit_seed, unit_name, unit_sex, unit_faction, this.weapon);

        return null;
    }
    private string generateName()
    {
        int name_length = 0;
        this.unit_name = "";
        int unit_faction_range = (int)UnityEngine.Random.Range(0, unit_factions.Count);
        //Debug.Log("unit_faction_range: " + unit_faction_range);
        unit_faction = unit_factions[unit_faction_range];

        int sex_range = ((int)UnityEngine.Random.Range(0, 2));
        //Debug.Log("sex_range: " + sex_range);
        if (sex_range > 0)
        {
            this.unit_sex = "Male";
        }
        else
        {
            this.unit_sex = "Female";
        }


        List<string> end_str = new List<string>();

        switch (unit_faction)
        {
            case FactionName.Alumi:
                name_length = nameLengthGenerator(50, 25, 20); //50% 1, 25% 2, 20% 3, 5% 4
                if (unit_sex.Equals("Male"))
                {
                    end_str.Add("ma");
                }
                else
                {
                    end_str.Add("is"); end_str.Add("sa");
                }
                break;
            case FactionName.Vyrzir:
                name_length = nameLengthGenerator(80, 15, 3);
                if (unit_sex.Equals("Male"))
                {
                    end_str.Add("uk"); end_str.Add("us");
                }
                else
                {
                    end_str.Add("na"); end_str.Add("iza"); end_str.Add("in");
                }
                break;
            case FactionName.Riven:
                name_length = nameLengthGenerator(50, 50, 0);
                if (unit_sex.Equals("Male"))
                {
                    end_str.Add("n"); end_str.Add("j");
                }
                else
                {
                    end_str.Add("n"); end_str.Add("j");
                }
                break;
            case FactionName.Gaorrul:
                name_length = nameLengthGenerator(0, 50, 30);
                if (unit_sex.Equals("Male"))
                {
                    end_str.Add("ool"); end_str.Add("uuh");
                }
                else
                {
                    end_str.Add("aek"); end_str.Add("aer"); end_str.Add("aem");
                }
                break;
            default:
                break;
        }
        for (int j = 1; j <= name_length; j++)
        {
            int body_range = (int)UnityEngine.Random.Range(0, body.Count);
            //Debug.Log("body_range: " + body_range);
            if (j == 1)
            {
                this.unit_name += body[body_range].ToUpper();
            }
            else
            {
                this.unit_name += body[body_range];
            }
            int vowels_range = (int)UnityEngine.Random.Range(0, vowels.Count);
            //Debug.Log("vowels_range: " + vowels_range);
            this.unit_name += vowels[(int)UnityEngine.Random.Range(0, vowels.Count)];
        }
        this.unit_name += end_str[(int)UnityEngine.Random.Range(0, end_str.Count)];

        return this.unit_name;
    }
    private int nameLengthGenerator(int one_weigth, int two_weigth, int three_weigth)
    {
        int name_lenght_seed = (int)UnityEngine.Random.Range(0, 100);
        if (name_lenght_seed < one_weigth)
        {
            return 1;
        }
        else if (name_lenght_seed < one_weigth + two_weigth)
        {
            return 2;
        }
        else if (name_lenght_seed < two_weigth + three_weigth)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }
    private Weapon generateWeapon()
    {
        //Debug.Log("Cenas");
        List<string> weapon_names = new List<string>();
        weapon_names.Add("Sword"); weapon_names.Add("Axe");
        weapon_names.Add("Bow"); weapon_names.Add("Gun");

        
        //Debug.Log("1");
        int weapon_seed = (int)UnityEngine.Random.Range(0, ListsManager.weaponsList.Count);
        //Debug.Log("1");
        ScriptableObject.CreateInstance(typeof(Weapon));
        //Debug.Log("2");
        Weapon w = (Weapon)ScriptableObject.CreateInstance(typeof(Weapon));
        //w.weapon_name = weapon_names[weapon_seed];
        //Debug.Log("weapon_seed: " + weapon_seed + " - " + w.weapon_name);
        return w;

        //Debug.Log("this.weapon.NAME: " + this.weapon.name);
    }
}

