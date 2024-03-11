
using UnityEngine;

namespace Mines
{
    public class HealthPickup : BasePickup
    {
        [SerializeField] private int healthRegen = 25;

        protected override void Interact(Collider2D other)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.AddHealth(healthRegen);
            }
        }
    }
}