using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ListsManager : MonoBehaviour
{
    public static ListsManager listManager = null;
    public static List<SkillSO> weaponsList = new List<SkillSO>();
    // Start is called before the first frame update
    void Start()
    {
        if(listManager == null) { listManager = this; } else { Destroy(this); }

        /*string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Prefabs/ScriptableObjects/Weapons" });
        weaponsList.Clear();
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var wp = AssetDatabase.LoadAssetAtPath<Weapon>(SOpath);
            weaponsList.Add(wp);
            //wp.Print();
        }
        */
        Object[] objects = Resources.LoadAll("ScriptableObjects/Weapons", typeof(SkillSO));

        foreach (Object obj in objects)
        {
            weaponsList.Add(obj as SkillSO);
        }
        Debug.Log("weaponsList Size: " + objects.Length);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}