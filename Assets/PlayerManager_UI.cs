using HelloWorld;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager_UI : NetworkBehaviour
{
    public TextMeshProUGUI hp;
    public TextMeshProUGUI actions;
    public HelloWorldPlayer hwp;

    public void refreshPlayerUI() {
        if (hwp == null) hwp = this.GetComponent<HelloWorldPlayer>();
        hp.text= hwp.hp.Value.ToString();
        actions.text = hwp.actions.Value.ToString();
    }
}
