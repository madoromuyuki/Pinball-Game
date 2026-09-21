using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    // Reference to the ball's rigid body
    Rigidbody2D myBody;

    // Reference to horizontal movement input
    InputAction move;

    // Horizontal movement speed
    public float moveSpeed = 8f;

    void Start()
    {
        // Get the Rigidbody2D attached to the ball
        myBody = GetComponent<Rigidbody2D>();

        // Get the Move action from the Input System
        move = InputSystem.actions.FindAction("Move");
    }

    void FixedUpdate()
    {
        // Read the Move input as a Vector2
        Vector2 moveInput = move.ReadValue<Vector2>();

        // Control horizontal velocity while keeping vertical physics unchanged
        myBody.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            myBody.linearVelocity.y
        );
    }
}
