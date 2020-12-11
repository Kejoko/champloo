using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public bool useVelocityMovement = true;
    public float fallMultiplier = 2.5f;
    public float movementForce = 10.0f;
    public float jumpForce = 5.0f;
    public int numberJumps = 2;

    public Text debugMessage;

    private bool isGrounded;
    private int remainingJumps;

    private PlayerInput playerInput;
    private Rigidbody rb;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    /*
    private void Start()
    {
        
    }
    */

    private void FixedUpdate()
    {
        debugMessage.text = "";

        Move();
        CheckGround();
        Jump();

        debugMessage.text += "Velocity: " + rb.velocity + '\n';
    }

    /*
    private void Update()
    {

    }
    */

    /*
     * How do we want movement to work? Here are a list of important questions
     * to consider:
     * - Should velocity or force be used?
     * 
     * Free rotating camera and player only moves "forwards" but in different
     * directions (Cube World, BOTW):
     *  - Should the maximum movement speed be reached automatically or should the
     *  player accelerate towards the maximum speed?
     *  
     * Camera trailing the player and always staying behind, allowing for
     * strafing and backpeddling (Risk Of Rain 2, Skyrim):
     *  - Should the player move in the same speed in all directions?
     *  - For each direction, should the maximum movement speed for that direction
     *  be reached automatically or should the player accelerate towards the
     *  maximum speed?
     *  - Assuming the different directions of movement have different maximum
     *  speeds, how should the maximum speed be calculated if the player is
     *  strafing and moving forwards/backwards? Should the maximum speed be a
     *  linear interpolation between the two depending on the angle of movement?
     *  Should it be the maximum/minumum of the two directional speeds?
    */
    private void Move()
    {
        if (useVelocityMovement) {
            // Use rigidbody velocity
            Vector2 movement = playerInput.movementInput;
            movement.Normalize();
            movement *= movementForce;
        } else {
            // Use rigidbody force
        }
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1.0f + 0.01f)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
        debugMessage.text += "Grounded: " + isGrounded + '\n';
    }

    /*
     * How should jumping work? Important questions:
     * - Use velocity or force on rigidbody?
     * - What do we want the player to be able to jump off? This is dependant on
     * what the player is able to stand/move on.
    */
    private void Jump()
    {
        if (isGrounded) {
            remainingJumps = numberJumps;
        }

        debugMessage.text += "Remaining jumps: " + remainingJumps + '\n';

        if (playerInput.jumpInput && remainingJumps > 0) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            remainingJumps -= 1;
        }

        if (rb.velocity.y < 0) {
            rb.useGravity = false;
            rb.AddForce(Physics.gravity * fallMultiplier, ForceMode.Acceleration);
        } else {
            rb.useGravity = true;
        }

        debugMessage.text += "Use gravity: " + rb.useGravity + '\n';
    }
}