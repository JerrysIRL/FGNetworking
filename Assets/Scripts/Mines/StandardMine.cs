using UnityEngine;

namespace Mines
{
    public class StandardMine : BasePickup
    {
        [SerializeField] private int mineDamage = 25;

        public override void Interact(Collider2D other)
        {
            if (IsServer)
            {
                Health health = other.GetComponent<Health>();
                if (!health) return;
                health.TakeDamage(mineDamage);

                int xPosition = Random.Range(-4, 4);
                int yPosition = Random.Range(-2, 2);

                transform.position = new Vector3(xPosition, yPosition, 0);
            }
        }
    }
}