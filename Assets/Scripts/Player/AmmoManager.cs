using Unity.Netcode;
using UnityEngine;

public class AmmoManager : NetworkBehaviour
{
    [SerializeField] private int startAmmo = 10;
    public NetworkVariable<int> ammoAmount = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        ammoAmount.Value = startAmmo;
    }

    public void RefillAmmo(int refillAmount)
    {
        ammoAmount.Value = Mathf.Clamp(ammoAmount.Value + refillAmount, 0, 100);
    }

    public void DecreaseAmmo()
    {
        ammoAmount.Value = Mathf.Clamp(ammoAmount.Value - 1, 0, 100);
    }
}