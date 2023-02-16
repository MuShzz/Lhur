using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;

    void Start()
    {
       
            netManager = GetComponentInParent<NetworkManager>();

        if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        if (args.TryGetValue("-mode", out string mode))
        {
            Debug.Log(" ############### mode: "+mode);
            switch (mode)
            {
                case "server":
                    if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Scenes/GameScene"))
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/GameScene");
                        return;
                    }
                    UnityTransport ut = NetworkManager.Singleton.GetComponent<UnityTransport>();
                    if (args.TryGetValue("-ip", out string cmd_ip)) ut.ConnectionData.Address = cmd_ip;
                    if (args.TryGetValue("-port", out string cmd_port)) ut.ConnectionData.Port = (ushort)int.Parse(cmd_port);
                    netManager.StartServer();
                    break;
                case "host":
                    netManager.StartHost();
                    break;
                case "client":
                    netManager.StartClient();
                    break;
            }
        }
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}