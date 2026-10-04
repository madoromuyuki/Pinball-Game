using UnityEngine;

public class MoveSideToSide : MonoBehaviour
{
    public Camera gameCamera;
    public float moveSpeed = 1f;

    Rigidbody2D myBody;
    BoxCollider2D myCollider;
    SpriteRenderer myRenderer;
    bool movingRight = false;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        myRenderer = GetComponent<SpriteRenderer>();

        Vector3 spriteSize = myRenderer.sprite.bounds.size;
        float scaleX = Mathf.Abs(transform.localScale.x);
        float scaleY = Mathf.Abs(transform.localScale.y);
        float squareSize = Mathf.Max(spriteSize.x * scaleX, spriteSize.y * scaleY);
        myCollider.size = new Vector2(squareSize / scaleX, squareSize / scaleY);
        myCollider.offset = myRenderer.sprite.bounds.center;

        UpdateFacing();
    }

    void FixedUpdate()
    {
        float leftEdge = gameCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, 10f)).x;
        float rightEdge = gameCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 10f)).x;
        float halfWidth = myCollider.bounds.extents.x;
        float leftLimit = leftEdge + halfWidth;
        float rightLimit = rightEdge - halfWidth;
        Vector2 newPosition = myBody.position;
        float movement = moveSpeed * Time.fixedDeltaTime;

        if (movingRight)
        {
            newPosition.x = newPosition.x + movement;
            if (newPosition.x >= rightLimit)
            {
                newPosition.x = rightLimit;
                movingRight = false;
            }
        }
        else
        {
            newPosition.x = newPosition.x - movement;
            if (newPosition.x <= leftLimit)
            {
                newPosition.x = leftLimit;
                movingRight = true;
            }
        }

        UpdateFacing();
        myBody.MovePosition(newPosition);
    }

    void UpdateFacing()
    {
        myRenderer.flipX = movingRight;
    }
}
