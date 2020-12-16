using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Lateral Movement")]
    public bool useVelocityMove = true;
    public float movementAcceleration = 0.5f;
    public float movementDrag = 5.0f;
    public float maxMovementSpeed = 10.0f;

    [Header("Vertical Movement")]
    public bool useVelocityJump = true;
    public int numberJumps = 2;
    public float jumpSpeed = 5.0f;
    public float fallMultiplier = 2.5f;
    private int remainingJumps;

    [Header("Physics & Collisions")]
    public float groundDistance = 1.0f;
    private bool isGrounded;

    [Header("Debug")]
    public bool debugOn = true;

    [Header("Components")]
    public Text debugMessage;
    private PlayerInput playerInput;
    private Rigidbody rb;


    private void ClearDebugMessages()
    {
        debugMessage.text = "";
    }

    private void AddDebugMessage(string message)
    {
        if (debugOn) {
            debugMessage.text += message + '\n';
        }
    }

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
        ClearDebugMessages();
        Move();
        CheckGround();
        Jump();
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
        Vector3 movement = new Vector3(playerInput.horizontalMovementInput, 0f, playerInput.verticalMovementInput);
        movement.Normalize();

        if (useVelocityMove) {
            if (isGrounded) {
                rb.velocity += movement * movementAcceleration;
            } else {
                rb.velocity += movement * movementAcceleration / 5;
            }

            Vector3 currMovement = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (currMovement.magnitude > maxMovementSpeed) {
                rb.velocity = currMovement.normalized * maxMovementSpeed + Vector3.up * rb.velocity.y;
            }

            currMovement.x = rb.velocity.x;
            currMovement.z = rb.velocity.z;
            if (movement.magnitude == 0 && currMovement.magnitude > 0) {
                rb.velocity -= currMovement * (movementAcceleration * 0.25f);
            }
        } else {
            Vector3 currMovement = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce((movement * maxMovementSpeed) - currMovement, ForceMode.VelocityChange);
        }

        AddDebugMessage("Position: " + transform.position);
        AddDebugMessage("Velocity: " + rb.velocity);
    }

    private void CheckGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundDistance + 0.01f)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        AddDebugMessage("Grounded: " + isGrounded);
    }

    /*
     * How should jumping work? Important questions:
     * - Use velocity or force on rigidbody?
     * - What do we want the player to be able to jump off? This is dependant on
     * what the player is able to stand/move on.
     * - Do we want there to be an input window? Must the player give frame
     * perfect inputs or should we allow a small window for inputs? If the player
     * inputs a jump 2 frames before being grounded should we jump the character
     * the frame they hit the ground?
    */
    private void Jump()
    {
        if (isGrounded) {
            remainingJumps = numberJumps;
        }

        AddDebugMessage("Remaining jumps: " + remainingJumps);

        if (useVelocityJump) {
            if (playerInput.jumpInput && remainingJumps > 0) {
                rb.velocity += new Vector3(0f, -rb.velocity.y + jumpSpeed, 0f);
                remainingJumps -= 1;
            }
        } else {
            if (playerInput.jumpInput && remainingJumps > 0) {
                rb.AddForce(new Vector3(0, -rb.velocity.y, 0), ForceMode.VelocityChange);
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
                remainingJumps -= 1;
            }
        }

        if (rb.velocity.y < 0) {
            rb.AddForce(Physics.gravity * (fallMultiplier - 1), ForceMode.Acceleration);
            AddDebugMessage("Gravity: " + Physics.gravity * fallMultiplier);
        } else {
            AddDebugMessage("Gravity: " + Physics.gravity);
        }

    }

    private void OnDrawGizmos()
    {
        if (debugOn) {
            // Ground detection
            if (isGrounded) {
                Gizmos.color = Color.red;
            } else {
                Gizmos.color = Color.white;
            }
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * groundDistance));
        }
    }
}