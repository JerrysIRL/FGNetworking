using Unity.Netcode;
using UnityEngine;

namespace PowerUp
{
    public class Shield : NetworkBehaviour
    {
        public readonly NetworkVariable<int> HitPoints = new NetworkVariable<int>(MaxValue);
        
        private SpriteRenderer _renderer;
        private const int MaxValue = 2;
        
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public override void OnNetworkSpawn()
        {
            HitPoints.OnValueChanged += OnShieldHit;
            if (!IsServer)
                return;
            HitPoints.Value = MaxValue;
        }

        public void ResetShield(int previousvalue, int newvalue)
        {
            _renderer.enabled = true;
            if (IsServer)
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