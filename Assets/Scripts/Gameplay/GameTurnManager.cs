using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class GameTurnManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameStateSingleton.instance == null) return;

        switch (GameStateSingleton.instance.gameType)
        {
            case GameType.Online:
                switch (GameStateSingleton.instance.onlineType)
                {
                    case OnlineType.Server:
                        NetworkManager.Singleton.StartServer();
                        break;
                    case OnlineType.Host:
                        NetworkManager.Singleton.StartHost();
                        break;
                    case OnlineType.Client:
                        NetworkManager.Singleton.StartClient();
                        break;
                }
                break;
            case GameType.Local:
                Debug.Log("LocalGame TBI");
                break;
            case GameType.MatchMaking:
                MMTicketer ticketer = (MMTicketer)FindObjectOfType(typeof(MMTicketer));
                if (ticketer != null)
                {
                    Debug.Log("ticketer.paired_assignment.Port: " + ticketer.paired_assignment.Port);
                    Debug.Log("ticketer.paired_assignment.Ip: " + ticketer.paired_assignment.Ip);
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                        ticketer.paired_assignment.Ip,  // The IP address is a string
                        (ushort)ticketer.paired_assignment.Port // The port number is an unsigned short
                    );
                    NetworkManager.Singleton.StartClient();
                }
                break;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
