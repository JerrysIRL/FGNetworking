using System;
using PowerUp;
using Unity.Netcode;
using UnityEngine;


[RequireComponent(typeof(RespawnManager))]
public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private RespawnManager respawnManager;
    [SerializeField] private Shield shield;

    private event Action Death = delegate {};

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>();

    private void RestoreHealth() => currentHealth.Value = maxHealth;
    
    public override void OnNetworkSpawn()
    {
        //Death += shield.ResetShield;
        Debug.Log("subscribed to reset shield : " + OwnerClientId);
        
        respawnManager.numberOfRespawns.OnValueChanged += shield.ResetShield;
        
        if (!IsServer) return;
        currentHealth.Value = maxHealth;
        Death += respawnManager.HandleDeath;
        Death += RestoreHealth;
    }

    public void TakeDamage(int damage)
    {
        if (shield.HitPoints.Value > 0)
        {
            shield.HitPoints.Value--;
            return;
        }

        currentHealth.Value = Mathf.Clamp(currentHealth.Value + damage, 0, maxHealth);
        if (currentHealth.Value == 0)
        {
            Death.Invoke();
        }
    }

    public void AddHealth(int healthAmount)
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value + healthAmount, 0, maxHealth);
    }

    public override void OnNetworkDespawn()
    {
        Death -= respawnManager.HandleDeath;
        Death -= RestoreHealth;
        //Death -= shield.ResetShield;
    }
}