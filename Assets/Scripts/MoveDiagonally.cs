using UnityEngine;

public class MoveDiagonally : MonoBehaviour
{
    public Camera gameCamera;
    public Collider2D floor;
    public float moveSpeed = 1f;

    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    float directionX = 1f;
    float directionY = 1f;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        Vector3 bottomLeft = gameCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 10f));
        Vector3 topRight = gameCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 10f));
        Vector3 halfSize = myCollider.bounds.extents;

        float leftLimit = bottomLeft.x + halfSize.x;
        float rightLimit = topRight.x - halfSize.x;
        float bottomLimit = floor.bounds.max.y + halfSize.y;
        float topLimit = topRight.y - halfSize.y;

        Vector2 newPosition = myBody.position;
        float movement = moveSpeed * Time.fixedDeltaTime / Mathf.Sqrt(2f);
        newPosition.x = newPosition.x + directionX * movement;
        newPosition.y = newPosition.y + directionY * movement;

        if (newPosition.x >= rightLimit)
        {
            newPosition.x = rightLimit;
            directionX = -1f;
        }
        else if (newPosition.x <= leftLimit)
        {
            newPosition.x = leftLimit;
            directionX = 1f;
        }

        if (newPosition.y >= topLimit)
        {
            newPosition.y = topLimit;
            directionY = -1f;
        }
        else if (newPosition.y <= bottomLimit)
        {
            newPosition.y = bottomLimit;
            directionY = 1f;
        }

        myBody.MovePosition(newPosition);
    }
}
