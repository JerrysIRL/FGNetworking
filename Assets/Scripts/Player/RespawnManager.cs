using System.Collections;
using Extensions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : NetworkBehaviour
{
    [SerializeField] private int numOfRespawns = 3;
    public NetworkVariable<int> numberOfRespawns = new NetworkVariable<int>();

    private Collider2D _collider2D;
    private SpriteRenderer _renderer;
    private const float FlashingDelay = 0.2f;

    public override void OnNetworkSpawn()
    {
        _collider2D = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        
        if (!IsServer)
            return;
        numberOfRespawns.Value = numOfRespawns;
    }

    public void HandleDeath()
    {
        if (numberOfRespawns.Value == 0)
        {
            //death
            NetworkManager.Shutdown();
            SceneManager.LoadScene("Splash");
            return;
        }

        numberOfRespawns.Value--;
        transform.SetRandomPositionOnScreen();
        RespawnFuncRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void RespawnFuncRpc()
    {
        StartCoroutine(Respawn(2));
    }

    private IEnumerator Respawn(int invulnerabilitySeconds)
    {
        _collider2D.enabled = false;
        float timeElapsed = 0;

        while (timeElapsed <= invulnerabilitySeconds)
        {
            _renderer.enabled = !_renderer.enabled;
            yield return new WaitForSeconds(FlashingDelay);
            timeElapsed += FlashingDelay;
        }

        _collider2D.enabled = true;
        _renderer.enabled = true;
    }
}