using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public Vector2 movementInput { get; private set; }
    public bool jumpInput { get; private set; }

    /*
    private void Start()
    {
        
    }
    */

    private void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpInput = Input.GetKeyDown(KeyCode.Space);
    }
}
