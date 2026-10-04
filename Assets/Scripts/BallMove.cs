using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    Rigidbody2D myBody;

    InputAction move;

    public float moveSpeed = 8f;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();

        move = InputSystem.actions.FindAction("Move");
    }

    void FixedUpdate()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();

        myBody.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            myBody.linearVelocity.y
        );
    }
}
