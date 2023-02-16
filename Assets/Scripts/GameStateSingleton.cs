using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSingleton : MonoBehaviour
{
    public static GameStateSingleton instance;
    public GameType gameType = GameType.Local;
    public OnlineType onlineType;  // 
    void Start()
    {
        if(instance == null){ instance = this; DontDestroyOnLoad(this.gameObject); }
        else { Object.Destroy(this); }

    }

}
