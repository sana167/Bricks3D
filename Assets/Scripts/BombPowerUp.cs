using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : PowerUp
{
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioManager audioManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        Explode();
    }

    private void Explode()
    {
        if (audioManager == null)
        {
            audioManager = FindAnyObjectByType<AudioManager>();
        }

        if (audioManager != null)
        {
            audioManager.PlayExplode();
        }

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        HashSet<Brick> affectedBricks = new HashSet<Brick>();

        foreach (Collider nearbyCollider in nearbyColliders)
        {
            if (nearbyCollider.TryGetComponent(out Brick brick))
            {
                affectedBricks.Add(brick);
            }
        }

        foreach (Brick brick in affectedBricks)
        {
            brick.DestroyFromPowerUp();
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
