using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ThrownBrick : MonoBehaviour
{
    [SerializeField] private float throwDuration = 0.6f;
    [SerializeField] private float arcHeight = 2f;

    public void ThrowTo(Vector3 targetPosition)
    {
        StartCoroutine(ThrowRoutine(targetPosition));
    }

    private IEnumerator ThrowRoutine(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float timer = 0f;

        if (TryGetComponent<Collider>(out var brickCollider))
        {
            brickCollider.enabled = false;
        }

        while (timer < throwDuration)
        {
            timer += Time.deltaTime;
            float t = timer / throwDuration;

            Vector3 flatPosition = Vector3.Lerp(startPosition, targetPosition, t);
            float arc = Mathf.Sin(t * Mathf.PI) * arcHeight;

            transform.position = flatPosition + Vector3.up * arc;

            yield return null;
        }

        transform.position = targetPosition;

        if (brickCollider != null)
        {
            brickCollider.enabled = true;
        }
    }
}