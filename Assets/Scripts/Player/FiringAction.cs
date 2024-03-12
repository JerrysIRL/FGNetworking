using Projectiles;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AmmoManager))]
public class FiringAction : NetworkBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject clientSingleBulletPrefab;
    [SerializeField] GameObject serverSingleBulletPrefab;
    [SerializeField] NetworkObject homingMissilePrefab;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] AmmoManager ammoManager;


    public override void OnNetworkSpawn()
    {
        playerController.OnFireEvent += Fire;
        playerController.MissileLaunchEvent += LaunchMissileRpc ;
    }

    [Rpc(SendTo.Server)]
    private void LaunchMissileRpc()
    {
        var missile = NetworkManager.SpawnManager.InstantiateAndSpawn(homingMissilePrefab, position: bulletSpawnPoint.position, rotation: bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(missile.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        missile.GetComponent<HomingMissile>().InitMissile(OwnerClientId);
    }

    private void Fire(bool isShooting)
    {
        if (isShooting && ammoManager.ammoAmount.Value > 0)
        {
            ShootBullet();
        }
    }

    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        GameObject bullet = Instantiate(serverSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
        ammoManager.DecreaseAmmo();
        
        ShootBulletClientRpc();
    }

    [ClientRpc]
    private void ShootBulletClientRpc()
    {
        if (IsOwner) return;
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(clientSingleBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        ShootBulletServerRpc();
    }

    public override void OnNetworkDespawn()
    {
        playerController.OnFireEvent -= Fire;
    }
}