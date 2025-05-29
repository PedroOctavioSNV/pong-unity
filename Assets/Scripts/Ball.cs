using System.Drawing;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public BallAudio ballAudio;
    public ParticleSystem collisionParticle;
    public float maxInitialAngle = 0.67f;
    public float maxCollisionAngle = 45f;
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
        EmitParticle(10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreZone scoreZone = collision.GetComponent<ScoreZone>();
        if (scoreZone)
        {
            GameManager.instance.OnScoreZoneReached(scoreZone.id);
            GameManager.instance.screenshake.StartShake(0.33f, 0.1f);
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
            EmitParticle(5);
            AdjustAngle(paddle, collision);
            GameManager.instance.screenshake.StartShake(0.1f, 0.05f);
        }

        Wall wall = collision.collider.GetComponent<Wall>();

        if (wall)
        {
            ballAudio.PlayWallSound();
            EmitParticle(2);
            GameManager.instance.screenshake.StartShake(0.033f, 0.033f);
        }
    }

    private void AdjustAngle(Paddle paddle, Collision2D collision)
    {
        // Debugs so we can actually see the lines.

        Vector2 median = Vector2.zero;
        foreach (ContactPoint2D point in collision.contacts)
        {
            median += point.point;
            //Debug.DrawRay(point.point, Vector3.right, UnityEngine.Color.red, 1f);
        }

        median /= collision.contactCount;
        //Debug.DrawRay(median, Vector3.right, UnityEngine.Color.cyan, 1f);

        // Calculate relative distance from center (between -1 and 1)
        float absoluteDistanceFromCenter = median.y - paddle.transform.position.y;
        float relativeDistanceFromCenter = absoluteDistanceFromCenter * 2 / paddle.GetHeight();

        // Calculate rotation using quaternion
        int angleSign = paddle.IsLeftPaddle() ? 1 : -1;
        float angle = relativeDistanceFromCenter * maxCollisionAngle * angleSign;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //Debug.DrawRay(median, Vector3.forward, UnityEngine.Color.yellow, 1f);

        // Calculate direction / velocity
        Vector2 direction = paddle.IsLeftPaddle() ? Vector2.right : Vector2.left;
        Vector2 velocity = rotation * direction * rigidbody2D.linearVelocity.magnitude;
        rigidbody2D.linearVelocity = velocity;
        //Debug.DrawRay(median, velocity, UnityEngine.Color.green, 1f);

        //Debug.Break();
    }

    private void EmitParticle(int amount)
    {
        collisionParticle.Emit(amount);
    }
}