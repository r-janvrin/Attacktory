using UnityEngine;
using UnityEngine.InputSystem;


public class playerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 movementDirection = Vector2.zero;
    public InputActionReference move;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get the player's input from the input system
        movementDirection = move.action.ReadValue<Vector2>();
        //movementDirection = new Vector2(1, 0);
        //actually move the player that distance
        
    }

    private void FixedUpdate()
    {
        //set velocity to the player's input
        rb.linearVelocity = new Vector2(movementDirection.x * moveSpeed, movementDirection.y * moveSpeed);
    }

    private void OnEnable()
    {
        move.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
    }
}
