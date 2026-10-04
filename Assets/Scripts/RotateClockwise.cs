using UnityEngine;

public class RotateClockwise : MonoBehaviour
{
    Rigidbody2D myBody;

    public float rotationSpeed = 60f;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();

        Sprite mySprite = GetComponent<SpriteRenderer>().sprite;
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        float colliderSize = mySprite.bounds.size.x;
        if (mySprite.bounds.size.y > colliderSize)
        {
            colliderSize = mySprite.bounds.size.y;
        }
        myCollider.size = new Vector2(colliderSize, colliderSize);
        myCollider.offset = mySprite.bounds.center;
    }

    void FixedUpdate()
    {
        float rotationAmount = rotationSpeed * Time.fixedDeltaTime;

        float newRotation = myBody.rotation - rotationAmount;

        myBody.MoveRotation(newRotation);
    }
}
