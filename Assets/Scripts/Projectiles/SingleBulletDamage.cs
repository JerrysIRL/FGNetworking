using UnityEngine;

public class SingleBulletDamage : MonoBehaviour
{
    [SerializeField] int damage = 5;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Health health))
            health.TakeDamage(-damage);
    }
}