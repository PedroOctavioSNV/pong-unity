using UnityEngine;

public class Ball : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public BallAudio ballAudio;
    public float maxInitialAngle = 0.67f;
    public float moveSpeed = 1.0f;
    public float startX = 0f;
    public float maxStartY = 4f;
    public float speedMultiplier = 1.1f;

    private void Start()
    {
        GameManager.instance.onReset += ResetBall;
        GameManager.instance.gameUI.onStartGame += ResetBall;
    }

    private void ResetBall()
    {
        ResetBallPosition();
        InitialPush();
    }

    private void InitialPush()
    {
        Vector2 direction = Random.value < 0.5f ? Vector2.left : Vector2.right;

        direction.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rigidbody2D.linearVelocity = direction * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreZone scoreZone = collision.GetComponent<ScoreZone>();
        if (scoreZone)
        {
            GameManager.instance.OnScoreZoneReached(scoreZone.id);
        }
    }

    private void ResetBallPosition()
    {
        float positionY = Random.Range(-maxStartY, maxStartY);
        Vector2 position = new Vector2(startX, positionY);
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Paddle paddle = collision.collider.GetComponent<Paddle>();

        if (paddle)
        {
            ballAudio.PlayPaddleSound();
            rigidbody2D.linearVelocity *= speedMultiplier;
        }

        Wall wall = collision.collider.GetComponent<Wall>();

        if (wall)
        {
            ballAudio.PlayWallSound();
        }
    }
}