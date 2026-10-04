using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballTemplate;
    public Camera gameCamera;
    public Collider2D floor;
    public SpriteRenderer background;
    public Text scoreText;
    public Text ballsText;
    public Button startButton;
    public AudioClip clickSound;
    public AudioClip pluckSound;
    public AudioClip errorSound;
    public AudioClip toggleSound;
    public float clickVolume = 1f;
    public float pluckVolume = 1f;
    public float errorVolume = 1f;
    public float toggleVolume = 1f;

    AudioSource myAudio;

    int score = 0;
    int ballsDropped = 0;
    int ballsFinished = 0;
    bool gameStarted = false;
    int startFrame;

    void Start()
    {
        myAudio = gameObject.AddComponent<AudioSource>();
        myAudio.playOnAwake = false;
        myAudio.spatialBlend = 0f;

        if (background == null || background.sprite == null ||
            scoreText == null || ballsText == null || startButton == null)
        {
            Debug.LogError("Game Manager is missing scene references. Reopen Assets/Scenes/SampleScene.unity to load Background and Game UI.");
            enabled = false;
            return;
        }

        ballTemplate.SetActive(false);
        scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        ballsText.font = scoreText.font;
        scoreText.text = "Score: 0";
        ballsText.text = "Jellyfish Left: 5";
    }

    void Update()
    {

        if (gameStarted == false || ballsDropped >= 5)
        {
            return;
        }

        if (Time.frameCount == startFrame)
        {
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            DropBall(mousePosition);
        }
    }

    public void StartGame()
    {
        PlaySound(clickSound, clickVolume);
        score = 0;
        ballsDropped = 0;
        ballsFinished = 0;
        gameStarted = true;
        startFrame = Time.frameCount;
        scoreText.text = "Score: 0";
        ballsText.text = "Jellyfish Left: 5";
        startButton.gameObject.SetActive(false);
    }

    void DropBall(Vector2 mousePosition)
    {
        if (gameCamera.pixelRect.Contains(mousePosition) == false)
        {
            return;
        }

        Vector3 viewportPosition = gameCamera.ScreenToViewportPoint(mousePosition);
        if (viewportPosition.y > 0.92f)
        {
            return;
        }

        Vector3 clickPosition = gameCamera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y, 10f));
        Vector3 topPosition = gameCamera.ViewportToWorldPoint(
            new Vector3(0.5f, 1f, 10f));

        SpriteRenderer jellyfishRenderer = ballTemplate.GetComponent<SpriteRenderer>();
        float leftWidth = Mathf.Abs(jellyfishRenderer.sprite.bounds.min.x);
        float rightWidth = Mathf.Abs(jellyfishRenderer.sprite.bounds.max.x);
        float edgeSpace = Mathf.Max(leftWidth, rightWidth) * ballTemplate.transform.localScale.x;
        float leftEdge = floor.bounds.min.x + edgeSpace;
        float rightEdge = floor.bounds.max.x - edgeSpace;
        float ballX = Mathf.Clamp(clickPosition.x, leftEdge, rightEdge);
        Vector3 ballPosition = new Vector3(ballX, topPosition.y + 3.7f, 0f);

        GameObject newBall = Instantiate(ballTemplate, ballPosition, Quaternion.identity);
        BallDrop ballScript = newBall.GetComponent<BallDrop>();
        ballScript.gameManager = this;
        newBall.SetActive(true);

        ballsDropped = ballsDropped + 1;
        int ballsLeft = 5 - ballsDropped;
        ballsText.text = "Jellyfish Left: " + ballsLeft;
    }

    public void FinishBall(bool reachedFloor)
    {
        if (reachedFloor)
        {
            score = score + 1;
            scoreText.text = "Score: " + score;
        }

        ballsFinished = ballsFinished + 1;
        if (ballsFinished == 5)
        {
            gameStarted = false;
            startButton.gameObject.SetActive(true);
        }
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        if (sound != null)
        {
            myAudio.PlayOneShot(sound, Mathf.Clamp01(volume));
        }
    }

}
