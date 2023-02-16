using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILoadingAnimation : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    private float dotTimer = 0.5f;

    void Start()
    {
        loadingText.text = "";
    }

    void Update()
    {
        dotTimer -= Time.deltaTime;
        if (dotTimer <= 0)
        {
            loadingText.text += ".";
            if (loadingText.text.Length > 10)
            {
                loadingText.text = "";
            }
            dotTimer = 0.5f;
        }
    }
}
