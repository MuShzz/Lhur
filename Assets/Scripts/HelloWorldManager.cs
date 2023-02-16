
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldManager : MonoBehaviour
    {
        void Start() {
            MMTicketer ticketer = (MMTicketer)FindObjectOfType(typeof(MMTicketer));
            if (ticketer != null)
            {
                Debug.Log("Found ticketer!!!!!");
                
                Debug.Log("ticketer.paired_assignment.Port: "+ ticketer.paired_assignment.Port);
                Debug.Log("ticketer.paired_assignment.Ip: " + ticketer.paired_assignment.Ip);
                //UnityTransport unity_transport = (UnityTransport)FindObjectOfType(typeof(UnityTransport));
                //unity_transport.ConnectionData.Address = ticketer.paired_assignment.Ip;
                //unity_transport.ConnectionData.Port = (ushort)ticketer.paired_assignment.Port;
            
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    ticketer.paired_assignment.Ip,  // The IP address is a string
                    (ushort)ticketer.paired_assignment.Port // The port number is an unsigned short
                );
                
                //NetworkManager.Singleton. = serverIp;
                //NetworkManager.Singleton.networkPort = serverPort;
                NetworkManager.Singleton.StartClient();
            }
        }
        void OnGUI()
        {
            
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            
            else
            {
                StatusLabels();
                SubmitNewPosition();
            }

            GUILayout.EndArea();


        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) {
                /*
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    "34.147.66.239",  // The IP address is a string
                    (ushort)9000 // The port number is an unsigned short
                );
                */
                NetworkManager.Singleton.StartClient(); 
            
            }
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
            //Debug.Log("mode: "+ mode);
        }

        static void SubmitNewPosition()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<HelloWorldPlayer>().Move();
                }
                else
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<HelloWorldPlayer>();
                    player.Move();
                }
                /*
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    NetworkObject pObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid);
                    pObject.GetComponent<PlayerManager_UI>().refreshPlayerUI();
                }
                */
                
            }
        }
    }
}