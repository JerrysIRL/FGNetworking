using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Lobbies.Models;

public class HostManager : ScriptableObject
{
    public string joinCode = "";
    private const int MaxConnections = 20;

    public string lobbyID = "";

    Allocation allocation;


    public async Task<bool> InitSeverAsync()
    {
        await Task.Delay(0);
        return true;
    }


    public async Task StartHostAsync()
    {
        allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);

        if (allocation != null) joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log("JoinCode: " + joinCode);

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport == null) return;

        RelayServerData relayServerData = new RelayServerData(allocation, "udp"); // dtls
        transport.SetRelayServerData(relayServerData);
        string playerName = PlayerPrefs.GetString("userName");
        string lobbyName = $"{playerName}'s Lobby";
        // We gonna create lobby options
        CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions();
        createLobbyOptions.IsLocked = false;
        createLobbyOptions.IsPrivate = false;
        createLobbyOptions.Data = new Dictionary<string, DataObject>()
        {
            { "JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Public, joinCode) },
            { "LobbyName", new DataObject(visibility: DataObject.VisibilityOptions.Public, lobbyName) }
        };
        var lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, MaxConnections, createLobbyOptions);
        lobbyID = lobby.Id;

        NetworkServer networkServer = new NetworkServer(NetworkManager.Singleton);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = UserDataWrapper.PayLoadInBytes();

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public async void PingServer()
    {
        await Lobbies.Instance.SendHeartbeatPingAsync(lobbyID);
        Debug.Log("Pinging Self");
    }
}