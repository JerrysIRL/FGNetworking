using UnityEngine;

namespace Mines
{
    public class StandardMine : BasePickup
    {
        [SerializeField] private int mineDamage = 25;

        protected override void Interact(Collider2D other)
        {
            if (IsServer)
            {
                Health health = other.GetComponent<Health>();
                if (!health) return;
                health.ChangeHealth(-mineDamage);

                SetRandomLocation();
            }
        }
    }
}