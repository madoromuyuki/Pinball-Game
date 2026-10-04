using UnityEngine;

public class BallDrop : MonoBehaviour
{
    public GameManager gameManager;
    bool finished = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (finished)
        {
            return;
        }

        if (collision.gameObject == gameManager.floor.gameObject)
        {
            gameManager.PlaySound(gameManager.toggleSound, gameManager.toggleVolume);
            FinishBall(true);
        }
        else if (collision.gameObject.GetComponent<MoveSideToSide>() != null)
        {
            gameManager.PlaySound(gameManager.errorSound, gameManager.errorVolume);
            FinishBall(false);
        }
        else if (collision.gameObject.GetComponent<RotateClockwise>() != null)
        {
            gameManager.PlaySound(gameManager.pluckSound, gameManager.pluckVolume);
        }
    }

    void Update()
    {
        if (transform.position.y < gameManager.floor.bounds.min.y - 2f)
        {
            FinishBall(false);
        }
    }

    void FinishBall(bool reachedFloor)
    {
        if (finished)
        {
            return;
        }

        finished = true;
        gameManager.FinishBall(reachedFloor);
        Destroy(gameObject);
    }
}
