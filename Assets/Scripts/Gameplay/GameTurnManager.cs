using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class GameTurnManager : NetworkBehaviour
{
    public static GameTurnManager instance;
    public PlayerS myPlayer;
    public PlayerS enemyPlayer;
    public NetworkVariable<int> playerTurn;
    public NetworkVariable<int> playerCount;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(this.gameObject); }
        else { Object.Destroy(this); }

        if (GameStateSingleton.instance == null) return;

        OnlineType onlineType = GameStateSingleton.instance.onlineType;
        playerCount.Value = 0;
        if(onlineType == OnlineType.Server || onlineType == OnlineType.Host) { playerTurn.Value = 1;  }
        
        switch (GameStateSingleton.instance.gameType)
        {
            case GameType.Online:
                switch (onlineType)
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

    public void SpawnPlayer(PlayerS pl)
    {
        Debug.Log("GameTurnManager SpawnPlayer | STARTING - "+ NetworkManager.Singleton.IsServer);
        if (NetworkManager.Singleton.IsClient == true) { return; }
        
        int playerCount = NetworkManager.Singleton.ConnectedClientsIds.Count;
        Debug.Log("GameTurnManager SpawnPlayer | " + playerCount);
        pl.playerNumber.Value = playerCount;

    }
    public void BackAction() { 
    
        if(myPlayer == null){ return; }

        if(myPlayer.selectedSkill != null)
        {
            myPlayer.playerUI.skillMenuUI.UnSelectAll();
            myPlayer.selectedSkill = null;
        }
        else if(myPlayer.selectedUnit != null)
        {
            myPlayer.selectedUnit.PlayerDeselectUnit();
            myPlayer.RefreshMenuUI(null);
            myPlayer.selectedUnit = null;   
        }
    }
    public void ChangePlayerTurn()
    {
        if(!NetworkManager.Singleton.IsServer == false) { return; }

        if (playerTurn.Value == 1)
        {
            playerTurn.Value = 2;
        }
        else
        {
            playerTurn.Value = 1;
        }
    }
    public PlayerS GetPlayer(int playerNumber) { 
        if(myPlayer.playerNumber.Value == playerNumber)
        {
            return myPlayer;
        }
        else if (enemyPlayer.playerNumber.Value == playerNumber)
        {
            return enemyPlayer;
        }
        return null; 
    }

    #region Keyboard
    void OnKeyDown(KeyDownEvent ev)
    {
        Debug.Log("Key DOWN!!! "+ ev.keyCode);
        if(ev.keyCode == KeyCode.Q || ev.keyCode == KeyCode.Q)
        {
            BackAction();
        }
    }
    void Update()
    {
        ListenKeyboard();

        if (NetworkManager.Singleton.IsServer == false) { return; }
        
    }
    private void ListenKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            BackAction();
        }
    }
    #endregion

}
