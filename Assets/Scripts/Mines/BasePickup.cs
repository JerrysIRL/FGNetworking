using Extensions;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Mines
{
    [RequireComponent(typeof(Collider2D), typeof(NetworkObject), typeof(NetworkTransform))]
    public abstract class BasePickup : NetworkBehaviour
    {
        private void Start()
        {
            GetComponent<Collider2D>().isTrigger = true;
            transform.SetRandomPositionOnScreen();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || !IsServer) 
                return;
            Interact(other);
            transform.SetRandomPositionOnScreen();
        }

        protected abstract void Interact(Collider2D otherCollider);
    }
}