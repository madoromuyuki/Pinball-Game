using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class GameLayout : MonoBehaviour
{
    public Camera gameCamera;
    public SpriteRenderer background;
    public SpriteRenderer floor;
    public BoxCollider2D floorCollider;
    public Text scoreText;
    public Text ballsText;

    void Update()
    {
        Font gameFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (scoreText != null && scoreText.font != gameFont)
        {
            scoreText.font = gameFont;
        }
        if (ballsText != null && ballsText.font != gameFont)
        {
            ballsText.font = gameFont;
        }

        if (gameCamera == null || background == null || floor == null)
        {
            return;
        }
        if (background.sprite == null || floor.sprite == null)
        {
            return;
        }

        gameCamera.rect = new Rect(0f, 0f, 1f, 1f);
        gameCamera.transform.position = new Vector3(0f, 0f, -10f);
        gameCamera.orthographicSize = 12f;

        float viewHeight = gameCamera.orthographicSize * 2f;
        float viewWidth = viewHeight * gameCamera.aspect;

        Vector3 backgroundSize = background.sprite.bounds.size;
        float backgroundScale = viewWidth / backgroundSize.x;
        float heightScale = viewHeight / backgroundSize.y;
        if (heightScale > backgroundScale)
        {
            backgroundScale = heightScale;
        }
        background.transform.localScale = new Vector3(backgroundScale, backgroundScale, 1f);
        Vector3 backgroundCenter = background.sprite.bounds.center;
        background.transform.position = new Vector3(
            -backgroundCenter.x * backgroundScale,
            -backgroundCenter.y * backgroundScale,
            5f);

        Vector3 floorSize = floor.sprite.bounds.size;
        float floorScaleX = viewWidth / floorSize.x;
        float floorScaleY = 2.72f / floorSize.y;
        floor.transform.localScale = new Vector3(floorScaleX, floorScaleY, 1f);
        Vector3 floorCenter = floor.sprite.bounds.center;
        float floorY = -viewHeight / 2f + 2.72f / 2f;
        floor.transform.position = new Vector3(
            -floorCenter.x * floorScaleX,
            floorY - floorCenter.y * floorScaleY,
            0f);

        if (floorCollider != null)
        {
            floorCollider.size = new Vector2(floorSize.x, floorSize.y);
            floorCollider.offset = floorCenter;
        }
    }
}
