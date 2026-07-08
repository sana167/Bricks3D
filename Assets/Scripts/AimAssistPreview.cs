using UnityEngine;

public class AimAssistPreview : MonoBehaviour
{
    [SerializeField] private BallMovement ballMovement;
    [SerializeField] private Rigidbody ballRb;
    [SerializeField] private Transform ball;
    [SerializeField] private Transform paddle;
    [SerializeField] private Collider paddleCollider;

    [SerializeField] private LineRenderer ballToPaddleLine;
    [SerializeField] private LineRenderer paddleToTargetLine;

    [SerializeField] private LayerMask paddleLayer;
    [SerializeField] private LayerMask targetLayer;

    [SerializeField] private float previewDistance = 50f;
    [SerializeField] private float activeDuration = 10f;

    private bool isActive = false;
    private float timer = 0f;

    private void Start()
    {
        HidePreview();
    }

    private void Update()
    {
        if (!isActive)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Deactivate();
            return;
        }

        DrawPreview();
    }

    public void Activate()
    {
        isActive = true;
        timer = activeDuration;
    }

    public void Deactivate()
    {
        isActive = false;
        HidePreview();
    }

    private void DrawPreview()
    {
        if (ballRb.linearVelocity.z <= 0f)
        {
            HidePreview();
            return;
        }

        Vector3 ballDirection = ballRb.linearVelocity.normalized;

        if (!Physics.Raycast(ball.position, ballDirection, out RaycastHit paddleHit, previewDistance, paddleLayer))
        {
            HidePreview();
            return;
        }

        Vector3 predictedBounceDirection =
            ballMovement.GetPredictedPaddleBounceDirection(
                paddle.position,
                paddleCollider.bounds.size.x,
                paddleHit.point
            );

        ballToPaddleLine.enabled = true;
        ballToPaddleLine.positionCount = 2;
        ballToPaddleLine.SetPosition(0, ball.position);
        ballToPaddleLine.SetPosition(1, paddleHit.point);

        if (Physics.Raycast(paddleHit.point, predictedBounceDirection, out RaycastHit targetHit, previewDistance, targetLayer))
        {
            paddleToTargetLine.enabled = true;
            paddleToTargetLine.positionCount = 2;
            paddleToTargetLine.SetPosition(0, paddleHit.point);
            paddleToTargetLine.SetPosition(1, targetHit.point);
        }
        else
        {
            paddleToTargetLine.enabled = true;
            paddleToTargetLine.positionCount = 2;
            paddleToTargetLine.SetPosition(0, paddleHit.point);
            paddleToTargetLine.SetPosition(1, paddleHit.point + predictedBounceDirection * previewDistance);
        }
    }

    private void HidePreview()
    {
        ballToPaddleLine.enabled = false;
        paddleToTargetLine.enabled = false;
    }
}