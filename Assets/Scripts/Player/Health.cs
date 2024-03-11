using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();


    public override void OnNetworkSpawn()
    {
        if(!IsServer) return;
        currentHealth.Value = 100;
    }

    public void ChangeHealth(int healthAmount)
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value + healthAmount, 0, 100);
        if (currentHealth.Value == 0)
        {
            NetworkObject.Despawn();
        }
    }

}
