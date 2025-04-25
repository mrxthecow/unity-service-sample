using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using com.buho.NetworkPack.Scene;

public class AutoMatchmaker : MonoBehaviour
{
    private Lobby currentLobby;
    private const int maxPlayers = 2;
    private bool isHost = false;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (SceneTransitionManager.Instance != null) SceneTransitionManager.Instance.FadeOut();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void StartMatchmaking()
    {
        UIManagerMenu.instance.SetMessage(true, "Matching...");
        QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync(new QueryLobbiesOptions
        {
            Filters = new List<QueryFilter>
            {
                new QueryFilter(
                field: QueryFilter.FieldOptions.AvailableSlots,
                op: QueryFilter.OpOptions.GT,
                value: "0"
                )
            }
        });

        if (response.Results.Count > 0)
        {
            await JoinLobbyAsClient(response.Results[0]);
        }
        else
        {
            await CreateLobbyAsHost();
        }
    }

    private async Task CreateLobbyAsHost()
    {
        Debug.Log("Create Lobby");
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        currentLobby = await LobbyService.Instance.CreateLobbyAsync("AutoRoom", maxPlayers, new CreateLobbyOptions
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>
            {
                { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
            }
        });

        StartCoroutine(KeepLobbyAlive(currentLobby.Id));

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetRelayServerData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.ConnectionData);

        NetworkManager.Singleton.StartHost();
        isHost = true;
        StartCoroutine(WaitForAllClientsThenLoadGame());
    }

    private async Task JoinLobbyAsClient(Lobby lobby)
    {
        currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id);
        string joinCode = currentLobby.Data["joinCode"].Value;

        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetRelayServerData(
            joinAllocation.RelayServer.IpV4,
            (ushort)joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData
        );

        NetworkManager.Singleton.StartClient();
        isHost = false;
        Debug.Log("Join Lobby");
    }

    private IEnumerator KeepLobbyAlive(string lobbyId)
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
        }
    }
    private IEnumerator WaitForAllClientsThenLoadGame()
    {
        Debug.Log("Wating for new player to join...");
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClients.Count >= 2)
            {
                Debug.Log("Connected");
                SceneTransitionManager.Instance.LoadNetworkedScene("Game");
                yield break;
            }
        }
    }
}
