using UnityEngine;

namespace Mines
{
    public class AmmoPickup : BasePickup
    {
        [SerializeField] private int refillAmount = 10;
        
        protected override void Interact(Collider2D otherCollider)
        {
            if (!IsServer) return; 
            
            if (otherCollider.TryGetComponent(out AmmoManager ammoManager))
            {
                ammoManager.RefillAmmo(refillAmount);
                Debug.Log(ammoManager.ammoAmount.Value);
                SetRandomLocation();
            }
        }

    }
}
