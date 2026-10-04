using UnityEngine;
using System.Collections.Generic;

public class JellyfishAnimation : MonoBehaviour
{
    public Sprite[] frames;
    public float frameTime = 0.1875f;

    SpriteRenderer myRenderer;
    PolygonCollider2D myCollider;
    float timer = 0f;
    int frameNumber = 0;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<PolygonCollider2D>();
        ShowFrame();
    }

    void Update()
    {
        timer = timer + Time.deltaTime;

        if (timer >= frameTime)
        {
            timer = timer - frameTime;
            frameNumber = frameNumber + 1;

            if (frameNumber >= frames.Length)
            {
                frameNumber = 0;
            }

            ShowFrame();
        }
    }

    void ShowFrame()
    {
        Sprite currentSprite = frames[frameNumber];
        myRenderer.sprite = currentSprite;

        int shapeCount = currentSprite.GetPhysicsShapeCount();
        if (shapeCount > 0)
        {
            myCollider.pathCount = shapeCount;
            for (int i = 0; i < shapeCount; i++)
            {
                List<Vector2> points = new List<Vector2>();
                currentSprite.GetPhysicsShape(i, points);
                myCollider.SetPath(i, points);
            }
        }
    }
}
