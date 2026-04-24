using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private MonoBehaviour movementComponent;
    [SerializeField] private MonoBehaviour groundCheckerComponent;

    private IMovement movement;
    private IGroundChecker groundChecker;

    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float jumpForce = 5.5f;

    private Vector2 moveInput;
    private bool isRunning;
    private bool jumpRequested;

    void Awake()
    {
        movement = movementComponent as IMovement;
        groundChecker = groundCheckerComponent as IGroundChecker;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        jumpRequested = false;
    }

    void HandleMovement()
    {
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 dir = forward * moveInput.y + right * moveInput.x;
        float speed = isRunning ? runSpeed : walkSpeed;

        movement.Move(dir, speed);
    }

    void HandleJump()
    {
        if (jumpRequested && groundChecker.IsGrounded())
        {
            movement.Jump(jumpForce);
        }
    }

    // Input에서 호출
    public void SetMoveInput(Vector2 input) => moveInput = input;
    public void SetRunning(bool value) => isRunning = value;
    public void RequestJump() => jumpRequested = true;
}