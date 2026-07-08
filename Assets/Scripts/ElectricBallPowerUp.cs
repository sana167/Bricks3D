using UnityEngine;

public class ElectricBallPowerUp : PowerUp
{
    [SerializeField] private float electricDuration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        
        if (other.TryGetComponent<BallMovement>(out var ball))
        {
            ball.ActivateElectricBall(electricDuration);
        }

        Destroy(gameObject);
    }
}