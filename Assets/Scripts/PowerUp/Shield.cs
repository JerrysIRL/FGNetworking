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
            if (!IsServer)
                return;
            
            HitPoints.OnValueChanged += OnShieldHit;
            HitPoints.Value = MaxValue;
        }

        public void ResetShield()
        {
            _renderer.enabled = true;
            HitPoints.Value = MaxValue;
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