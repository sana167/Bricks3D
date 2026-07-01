using UnityEngine;
using UnityEngine.InputSystem; // NEW: The Input System namespace

// We implement the interfaces that correspond to the actions we want to listen to.
// I can only listen for 'Move' and 'Launch' actions that have been set up in the asset.
public class InputHandler : MonoBehaviour, PlayerInput.IPlayerActions
{
    // The auto-generated class from the Input Action asset (GameInput)
    private PlayerInput playerInput;
    
    // Public variables to pass values to the PaddleController
    public Vector2 moveValue { get; private set; } = Vector2.zero;
    
    // Reference to the PaddleController script
    private PaddleController paddleController;
    [SerializeField] private BallMovement ball;
    [SerializeField] private GameManager gameManager;

    void Awake()
    {
        // 1. Initialize the auto-generated wrapper class
        playerInput = new PlayerInput();
        
        // 2. Assign this script as the listener for the 'Move' and 'Launch' actions
        playerInput.Player.SetCallbacks(this);

        // Get the paddle controller reference
        paddleController = GetComponent<PaddleController>();
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }
    
    // --- Implementation of PlayerInput.IMoveActions ---
    
    // This is called every frame input changes (A/D press/release, stick movement)
    public void OnMove(InputAction.CallbackContext context)
    {
        // Read the value (Vector2 for 2D Composite, Vector3 for 3D)
        moveValue = context.ReadValue<Vector2>();
        
        // Since the paddle only uses the X-axis for movement:
        if (paddleController != null)
        {
            paddleController.SetHorizontalInput(moveValue.x);
        }
    }
    
    // --- Implementation of PlayerInput.ILaunchActions ---

    // This is called when the Launch button (Space/Left Click) is pressed
    public void OnLaunch(InputAction.CallbackContext context)
    {
        // We only care about the moment the button is pressed
        if (context.performed && ball != null && gameManager != null)
        {
            // Logic Branching
            if (!ball.IsLaunched)
            {
                // If ball is on the paddle, launch it
                ball.TryLaunch();
            }
        }
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        // We only want to quit when the button is first pressed (performed)
        if (context.performed)
        {
            if (gameManager != null)
            {
                gameManager.RequestQuit();
            }
        }
    }
}