using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : BasePickup
{
    [SerializeField] private int healthRegen = 25;
    
    public override void Interact(Collider2D other)
    {
        base.Interact(other);
        if (IsServer)
        {
            Health health = other.GetComponent<Health>();
            if (!health) return;
            health.ReceiveHealth(healthRegen);

            int xPosition = Random.Range(-4, 4);
            int yPosition = Random.Range(-2, 2);

        }
    }
    
    
}
