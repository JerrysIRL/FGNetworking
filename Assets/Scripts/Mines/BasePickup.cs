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
            SetRandomLocation();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Interact(other);
        }

        protected void SetRandomLocation()
        {
            int xPosition = Random.Range(-4, 4);
            int yPosition = Random.Range(-2, 2);

            transform.position = new Vector3(xPosition, yPosition, 0);
        }

        protected abstract void Interact(Collider2D otherCollider);
    }
}