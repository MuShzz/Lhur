using TMPro;
using Unity.Netcode;
using UnityEngine;


namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<int> hp = new NetworkVariable<int>(10);
        public NetworkVariable<int> actions = new NetworkVariable<int>(0);

        public PlayerManager_UI ui_manager;
        public int pos = -45;
        public override void OnNetworkSpawn()
        {
            ui_manager = GetComponent<PlayerManager_UI>();
            NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            if (NetworkManager.Singleton.IsServer) {
                NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds) {
                    NetworkObject pObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid);
                    pObject.transform.position = new Vector3(pos, pObject.transform.position.y, pObject.transform.position.z);
                    pos += 90;
                }
            }
            else
            {
                if (this.IsOwner)
                {
                    transform.position = new Vector3(-45, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(45, transform.position.y, transform.position.z);
                }
            }
            
            Debug.Log("Checking how many times OnNetworkSpawn is called ------------");
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                //hp.Value--;
                //actions.Value++;
                SubmitPositionRequestClientRpc();
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        
        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            hp.Value--;
            actions.Value++;
            //if(NetworkManager.Singleton.IsServer) 
            SubmitPositionRequestClientRpc();
        }
        
        [ClientRpc]
        void SubmitPositionRequestClientRpc(ClientRpcParams rpcParams = default)
        {
            this.ui_manager.refreshPlayerUI();
        }
    }
}