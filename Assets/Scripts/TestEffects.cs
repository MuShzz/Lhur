using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffects : MonoBehaviour
{
    public GameObject visualEffect;
    public GameObject unit;

    void Start()
    {
        VisualEffect();
    }

    
    void Update()
    {
        
    }
    public void VisualEffect()
    {
        RectTransform unitT = unit.GetComponent<RectTransform>();
        Vector2 unitScreenPos = new Vector2(unitT.anchoredPosition.x, unitT.anchoredPosition.y);

        //Vector2 screenPosition = RectTransformUtility.(Camera.main, unit.GetComponent<RectTransform>().transform.position);


        Vector3 screenPos;// = Camera.main.ScreenToViewportPoint(unit.transform.position); //Camera.main.transform.InverseTransformPoint(unit.transform.position);
                          //RectTransformUtility.ScreenPointToWorldPointInRectangle(unit.GetComponent<RectTransform>(), screenPosition, null, out screenPos);
                          //Debug.Log("screenPosition: " + screenPosition);


        //screenPos.z = 20;
        visualEffect.transform.localScale = new Vector3(100, 100, 10);
        GameObject go = Instantiate(visualEffect, unit.transform.position, visualEffect.transform.rotation, unit.transform);
        //go.transform.localPosition = Vector3.zero;
    }
}
