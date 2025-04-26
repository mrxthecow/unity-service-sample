using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
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
            NotifyClientsToExitClientRpc();
            ExitToLobby();
        }
        else
        {
            RequestExitServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestExitServerRpc(ServerRpcParams rpcParams = default)
    {
        NotifyClientsToExitClientRpc();
        ExitToLobby();
    }

    [ClientRpc]
    private void NotifyClientsToExitClientRpc()
    {
        if (!IsHost)
        {
            ExitToLobby();
        }
    }

    private void ExitToLobby()
    {
        NetworkManager.Singleton.Shutdown();
        SceneTransitionManager.Instance.LocalScreenTransition("Menu");
    }
    //------------------------------------------------------
    private Dictionary<ulong, int> playerChoices = new Dictionary<ulong, int>(); // clientId -> 0,1,2

    public void Select(int choice)
    {
        SubmitChoiceServerRpc(choice);
        UIManagerGame.instance.CloseBtn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SubmitChoiceServerRpc(int choice, ServerRpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;
        playerChoices[senderId] = choice;

        if (playerChoices.Count >= 2)
        {
            EvaluateWinner();
        }
    }

    private void EvaluateWinner()
    {
        var players = new List<ulong>(playerChoices.Keys);
        int choiceA = playerChoices[players[0]];
        int choiceB = playerChoices[players[1]];

        int result = Judge(choiceA, choiceB); // 0=draw, 1=player[0] wins, 2=player[1] wins

        ShowResultClientRpc(players[0], players[1], result);
        playerChoices.Clear();
    }

    private int Judge(int a, int b)
    {
        if (a == b) return 0;
        if ((a + 1) % 3 == b) return 2; // b win
        return 1; // a win
    }

    [ClientRpc]
    private void ShowResultClientRpc(ulong player0, ulong player1, int result)
    {
        ulong localId = NetworkManager.Singleton.LocalClientId;

        string message = "";
        if (result == 0) message = "平手";
        else if ((result == 1 && localId == player0) || (result == 2 && localId == player1))
            message = "你贏了！";
        else
            message = "你輸了！";

        Debug.Log($"結果：{message}");
        // 可以接著更新 UI 顯示
    }
}

