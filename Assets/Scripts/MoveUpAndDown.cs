using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    public Camera gameCamera;
    public Collider2D floor;
    public float moveSpeed = 0.6f;

    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    bool movingUp = true;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        Vector3 topPosition = gameCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 10f));
        float halfHeight = myCollider.bounds.extents.y;
        float topLimit = topPosition.y - halfHeight;
        float bottomLimit = floor.bounds.max.y + halfHeight;
        float movement = moveSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = myBody.position;

        if (movingUp)
        {
            newPosition.y = newPosition.y + movement;
            if (newPosition.y >= topLimit)
            {
                newPosition.y = topLimit;
                movingUp = false;
            }
        }
        else
        {
            newPosition.y = newPosition.y - movement;
            if (newPosition.y <= bottomLimit)
            {
                newPosition.y = bottomLimit;
                movingUp = true;
            }
        }

        myBody.MovePosition(newPosition);
    }
}
