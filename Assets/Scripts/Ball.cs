using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameManager gameManager;
    public new Rigidbody2D rigidbody2D;
    public float maxInitialAngle = 0.67f;
    public float moveSpeed = 1.0f;
    public float startX = 0f;
    public float maxStartY = 4f;

    private void Start()
    {
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
            gameManager.OnScoreZoneReached(scoreZone.id);
            Debug.Log("Scored a point!");
            ResetBall();
            InitialPush();
        }
    }

    private void ResetBall()
    {
        float positionY = Random.Range(-maxStartY, maxStartY);
        Vector2 position = new Vector2(startX, positionY);
        transform.position = position;
    }
}