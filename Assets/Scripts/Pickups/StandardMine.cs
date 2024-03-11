
using UnityEngine;

namespace Mines
{
    public class StandardMine : BasePickup
    {
        [SerializeField] private int mineDamage = 25;

        protected override void Interact(Collider2D other)
        {
            if (other.TryGetComponent(out Health health))
            {
                health.ChangeHealth(-mineDamage);
            }
        }
    }
}