using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public float horizontalMovementInput { get; private set; }
    public float verticalMovementInput { get; private set; }

    public bool debugInput { get; private set; }
    public bool jumpInput { get; private set; }

    /*
    private void Start()
    {
        
    }
    */

    private void Update()
    {
        horizontalMovementInput = Input.GetAxis("Horizontal");
        verticalMovementInput = Input.GetAxis("Vertical");

        jumpInput = Input.GetKeyDown(KeyCode.Space);

        debugInput = Input.GetKeyDown(KeyCode.P);
    }
}
