using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] GameObject InitialMenu;
    [SerializeField] GameObject PlayMenu;
    [SerializeField] GameObject PlayLocalMenu;

    //INITIAL
    public void Initial_PlayButton(){ transitionMenu(InitialMenu, PlayMenu);}

    //PLAY
    public void Play_OnlineButton() {
        if(AuthenticationService.Instance.AccessToken != null)
        {
            GameStateSingleton.instance.gameType = GameType.MatchMaking;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/MatchmakingScene");
        }
    }
    public void Play_LocalButton(){ transitionMenu(PlayMenu, PlayLocalMenu);}
    public void Play_BackButton(){ transitionMenu(PlayMenu, InitialMenu);}

    //PLAY_LOCAL
    public void PlayLocal_ServerButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/GameScene");

        GameStateSingleton.instance.gameType = GameType.Online;
        GameStateSingleton.instance.onlineType = OnlineType.Server;
        Debug.Log("After Server");
    }
    public void PlayLocal_ClientButton() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/GameScene");
        
        GameStateSingleton.instance.gameType = GameType.Online;
        GameStateSingleton.instance.onlineType = OnlineType.Client;
        Debug.Log("After Client");
    }
    public void PlayLocal_HostButton(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/GameScene");
        
        GameStateSingleton.instance.gameType = GameType.Online;
        GameStateSingleton.instance.onlineType = OnlineType.Host;
        Debug.Log("After Host");
    }
    public void PlayLocal_BackButton() { transitionMenu(PlayLocalMenu, PlayMenu); }


    private void transitionMenu(GameObject previous, GameObject next)
    {
        previous.SetActive(false);
        next.SetActive(true);
    }
}
