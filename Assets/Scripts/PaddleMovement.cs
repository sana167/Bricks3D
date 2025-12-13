using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public float maxZ = 4.5f; // Boundary limit
    public float minZ = -4.5f; // Boundary limit

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Calculate the new position
        Vector3 newPosition = transform.position + Vector3.left * horizontalInput * speed * Time.deltaTime;
        
        // Clamp the position to keep it within boundaries
        newPosition.z = transform.position.z; // Keep Z constant for 2D-like movement
        newPosition.x = Mathf.Clamp(newPosition.x, minZ, maxZ);

        transform.position = newPosition;
    }
}