using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

namespace PowerUp
{
    public class Shield : NetworkBehaviour
    {
        private SpriteRenderer _renderer;
        private const int MaxValue = 2;

        public readonly NetworkVariable<int> HitPoints = new NetworkVariable<int>();
        
        public override void OnNetworkSpawn()
        {
            _renderer = GetComponent<SpriteRenderer>();
            HitPoints.Value = MaxValue;
            HitPoints.OnValueChanged += OnShieldHit;
        }

        public void ResetShield()
        {
            Debug.LogError("Reset Shield" + OwnerClientId);
            Debug.LogError(_renderer);
            HitPoints.Value = MaxValue;
            _renderer.enabled = true;
        }

        private void OnShieldHit(int previousvalue, int newvalue)
        {
            if (newvalue == 0)
            {
                _renderer.enabled = false;
            }
        }

        public override void OnNetworkDespawn()
        {
            HitPoints.OnValueChanged -= OnShieldHit;
        }
    }
}