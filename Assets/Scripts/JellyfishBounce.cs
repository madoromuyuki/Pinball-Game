using UnityEngine;

public class JellyfishBounce : MonoBehaviour
{
    public Camera gameCamera;
    public float bounceForce = 1.2f;

    Rigidbody2D myBody;
    Collider2D myCollider;
    bool enteredScreen = false;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        Vector3 bottomLeft = gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 10f));
        Vector3 topRight = gameCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 10f));
        Bounds jellyfishBounds = myCollider.bounds;
        Vector2 position = myBody.position;
        Vector2 velocity = myBody.linearVelocity;

        if (jellyfishBounds.min.x <= bottomLeft.x && velocity.x <= 0f)
        {
            position.x = position.x + bottomLeft.x - jellyfishBounds.min.x + 0.01f;
            velocity.x = 0f;
            myBody.position = position;
            myBody.linearVelocity = velocity;
            myBody.AddForce(Vector2.right * bounceForce, ForceMode2D.Impulse);
        }
        else if (jellyfishBounds.max.x >= topRight.x && velocity.x >= 0f)
        {
            position.x = position.x - jellyfishBounds.max.x + topRight.x - 0.01f;
            velocity.x = 0f;
            myBody.position = position;
            myBody.linearVelocity = velocity;
            myBody.AddForce(Vector2.left * bounceForce, ForceMode2D.Impulse);
        }

        if (jellyfishBounds.max.y < topRight.y)
        {
            enteredScreen = true;
        }

        if (enteredScreen && jellyfishBounds.max.y >= topRight.y && velocity.y > 0f)
        {
            position = myBody.position;
            position.y = position.y - jellyfishBounds.max.y + topRight.y - 0.01f;
            velocity = myBody.linearVelocity;
            velocity.y = 0f;
            myBody.position = position;
            myBody.linearVelocity = velocity;
            myBody.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse);
        }
    }
}
