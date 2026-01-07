using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public float maxZ = 4.5f; 
    public float minZ = -4.5f; 

    private float currentHorizontalInput = 0f; // New variable to store input value

    // NEW: Public method called by the InputHandler
    public void SetHorizontalInput(float value)
    {
        currentHorizontalInput = value;
    }

    void Update()
    {
        // Use the stored input value instead of Input.GetAxis
        float horizontalInput = currentHorizontalInput;
        
        // ... (existing movement and clamping logic) ...
        Vector3 newPosition = transform.position + Vector3.left * horizontalInput * speed * Time.deltaTime;
        
        newPosition.z = transform.position.z; 
        newPosition.x = Mathf.Clamp(newPosition.x, minZ, maxZ);

        transform.position = newPosition;
    }
}