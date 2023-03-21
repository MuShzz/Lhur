using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill : Subject
{
    
    [SerializeField] PlayerS player;
    [SerializeField] public int position;

    public SkillSO skillSO;
    Camera cam;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void visualEffect(Unit unit)
    {
        //Vector3 screenPos = Camera.main.transform.InverseTransformPoint(unit.transform.position);
        //Instantiate(skillSO.visualEffect, screenPos, skillSO.visualEffect.transform.rotation);
        skillSO.visualEffect.transform.localScale = new Vector3(100, 100, 10);
        GameObject go = Instantiate(skillSO.visualEffect, unit.transform.position, skillSO.visualEffect.transform.rotation, unit.transform);
    }
}
