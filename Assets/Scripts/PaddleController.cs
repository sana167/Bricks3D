using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float maxX = 4.5f; 
    [SerializeField] private float minX = -4.5f; 

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
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        transform.position = newPosition;
    }
}