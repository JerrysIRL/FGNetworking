using Unity.Collections;
using Unity.Netcode;


public class Name : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> userNameNetwork = new NetworkVariable<FixedString128Bytes>();


    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        UserData userData = SavedClientInformationManager.GetUserData(OwnerClientId);
        userNameNetwork.Value = userData.userName;
    }
}