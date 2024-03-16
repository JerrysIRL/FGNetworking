using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode.Components;


namespace Projectiles
{
    [RequireComponent(typeof(Collider2D), typeof(NetworkTransform), typeof(Rigidbody2D))]
    public class HomingMissile : NetworkBehaviour
    {
        [Header("Variables")]
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float rotateSpeed = 150f;
        [SerializeField] private int damage = 50;
        
        private Transform TargetTransform { get; set; }
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void InitMissile(ulong ownerID)
        {
             TargetTransform = GetClosestEnemiesTransform(ownerID);
        }

        private Transform GetClosestEnemiesTransform(ulong ownerID)
        {
            var clients = NetworkManager.Singleton.ConnectedClients.Values.ToList();
            float closestDistance = float.MaxValue;
            Transform outTransform = null;
            foreach (var client in clients)
            {
                if (client.PlayerObject.OwnerClientId == ownerID)
                    continue;
                
                float distanceToClient = Vector3.Distance(transform.position, client.PlayerObject.transform.position);
                if (distanceToClient < closestDistance)
                {
                    closestDistance = distanceToClient; 
                    outTransform = client.PlayerObject.transform;
                }
            }

            return outTransform;
        }

        private void FixedUpdate()
        {
            if (!IsServer)
                return;
            
            if (!TargetTransform)
            {
                _rb.velocity = transform.up * (movementSpeed * Time.fixedDeltaTime);
                return;
            }

            Vector2 directionToTarget = (TargetTransform.position - transform.position).normalized;
            float rotateAmount = Vector3.Cross(directionToTarget, transform.up).z;

            _rb.angularVelocity = -rotateAmount * rotateSpeed;
            _rb.velocity = transform.up * (movementSpeed * Time.fixedDeltaTime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsServer)
                return;
            
            if (other.TryGetComponent(out Health health))
                health.TakeDamage(-damage);
            
            if(NetworkObject.IsSpawned)
                NetworkObject.Despawn();
        }
    }
}