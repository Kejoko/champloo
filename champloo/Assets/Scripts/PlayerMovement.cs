using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool useVelocityMovement = true;

    public float movementForce = 10.0f;
    public float jumpForce = 10.0f;
    public int numberJumps = 2;

    private int remainingJumps;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {

        Move();

        Jump();
    }

    private void Update()
    {
        
    }

    private void Move()
    {
        if (useVelocityMovement)
        {
            // Use rigidbody velocity
            Vector2 movement = playerInput.movementInput;
            movement.Normalize();
            movement *= movementForce;
        }
        else
        {
            // Use rigidbody force
        }
    }

    private bool IsGrounded()
    {
        return true;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            remainingJumps = numberJumps;
        }

        if (playerInput.jumpInput && remainingJumps > 0)
        {
            // Use rigidbody force
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            remainingJumps -= 1;
        }
    }
}
