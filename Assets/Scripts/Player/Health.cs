using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(RespawnManager))]
public class Health : NetworkBehaviour
{
    [SerializeField] private RespawnManager respawnManager;
    [SerializeField] private int maxHealth = 100; 

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        currentHealth.Value = maxHealth;
    }

    public void ChangeHealth(int healthAmount)
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value + healthAmount, 0, maxHealth);
        if (currentHealth.Value == 0)
        {
            respawnManager.HandleDeath();
            currentHealth.Value = maxHealth;
        }
    }
}