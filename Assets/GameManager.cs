using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using com.buho.NetworkPack.Scene;

public class GameManager : NetworkBehaviour
{
    private void Start()
    {
        if (SceneTransitionManager.Instance != null) SceneTransitionManager.Instance.FadeOut();
    }

    private void Update()
    {

    }
    public void Exit()
    {
        if (IsHost)
        {
            Debug.Log("[Exit] Host 離開，通知所有人...");
            NotifyClientsToExitClientRpc();
            ExitToLobby();
        }
        else
        {
            Debug.Log("[Exit] Client 要求退出");
            RequestExitServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestExitServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("[Exit] Host 收到 Client 請求退出");
        NotifyClientsToExitClientRpc();
        ExitToLobby();
    }

    [ClientRpc]
    private void NotifyClientsToExitClientRpc()
    {
        if (!IsHost)
        {
            Debug.Log("[Exit] Client 被通知離開");
            ExitToLobby();
        }
    }

    private void ExitToLobby()
    {
        NetworkManager.Singleton.Shutdown();
        SceneTransitionManager.Instance.LocalScreenTransition("Menu");
    }
}

