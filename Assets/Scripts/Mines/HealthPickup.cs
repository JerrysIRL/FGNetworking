using UnityEngine;

namespace Mines
{
    public class HealthPickup : BasePickup
    {
        [SerializeField] private int healthRegen = 25;

        protected override void Interact(Collider2D other)
        {
            if (IsServer)
            {
                Health health = other.GetComponent<Health>();
                if (!health) return;
                health.ChangeHealth(healthRegen);
                
                SetRandomLocation();
            }
        }
    }
}