using Unity.Netcode;
using UnityEngine;

public interface IInteractable
{
    public void Interact(Collider2D otherCollider);
}

[RequireComponent(typeof(Collider2D))]
public class BasePickup : NetworkBehaviour, IInteractable
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Interact(other);
    }

    public virtual void Interact(Collider2D other)
    {
        Debug.Log("Collided");
    }

}
