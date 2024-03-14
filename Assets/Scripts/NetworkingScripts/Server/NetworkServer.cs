using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    NetworkManager networkManager;

    public NetworkServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        networkManager.ConnectionApprovalCallback += ConnectionApproval;
        networkManager.OnClientDisconnectCallback += ClientDisconnect;
        networkManager.OnServerStopped += ShutdownServer;
    }

    private void ShutdownServer(bool obj)
    {
        HostSingelton.GetInstance().StopHostPing();
        Dispose();
    }

    private void ClientDisconnect(ulong networkID)
    {
        SavedClientInformationManager.RemoveClient(networkID);
    }

    private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(payload);
        userData.networkID = request.ClientNetworkId;

        response.Approved = true;

        SavedClientInformationManager.AddClient(userData);

        response.CreatePlayerObject = true; // Theo
    }

    public void Dispose()
    {
        if (networkManager != null)
        {
            Debug.Log("Disposing network server");
            networkManager.ConnectionApprovalCallback -= ConnectionApproval;
            NetworkManager.Singleton.OnClientDisconnectCallback -= ClientDisconnect;
            if (networkManager.IsListening) networkManager.Shutdown();
        }
    }
}