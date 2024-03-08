using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Mines
{

    [RequireComponent(typeof(Collider2D),
        typeof(NetworkObject),
        typeof(NetworkTransform))]
    public abstract class BasePickup : NetworkBehaviour
    {
        private void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Interact(other);
        }
        
        public abstract void Interact(Collider2D otherCollider);
    }
}