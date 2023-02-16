using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public string UserName;
    public TextMeshProUGUI userNameUI;
    public async void OnSignIn()
    {
        Debug.Log("OnSignIn");
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //AuthIdText.text = AuthenticationService.Instance.PlayerId;
        Debug.Log(AuthenticationService.Instance.AccessToken);
        userNameUI.text = AuthenticationService.Instance.PlayerId;
        //AuthButton.interactable = false;
        //FindMatchButton.interactable = true;
    }
}
