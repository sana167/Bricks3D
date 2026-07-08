using UnityEngine;

public class AimAssistPowerUp : PowerUp
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        AimAssistPreview preview = FindAnyObjectByType<AimAssistPreview>();

        if (preview != null)
        {
            preview.Activate();
        }

        Destroy(gameObject);
    }
}