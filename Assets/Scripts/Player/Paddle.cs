using UnityEngine;

public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public int id;
    public float moveSpeed = 2f;

    [Header("AI")]
    public float aiDeadzone = 1f;
    public float aiMoveSpeedMultiplierMin = 0.5f;
    public float aiMoveSpeedMultiplierMax = 1.5f;

    private Vector3 startPosition;
    private int direction = 0;
    private float moveSpeedMultiplier = 1f;

    private const string MovePlayer1InputName = "MovePlayer1";
    private const string MovePlayer2InputName = "MovePlayer2";

    private void Start()
    {
        startPosition = transform.position;
        GameManager.instance.onReset += ResetPosition;
    }

    private void ResetPosition()
    {
        transform.position = startPosition;
    }

    public void Update()
    {
        if (id == 2 && GameManager.instance.IsPlayer2Ai())
        {
            MoveAi();
        }
        else
        {
            float movement = GetInput();
            Move(movement);
        }
    }

    private void MoveAi()
    {
        Vector2 ballPosition = GameManager.instance.ball.transform.position;
        if (Mathf.Abs(ballPosition.y - transform.position.y) > aiDeadzone)
        {
            direction = ballPosition.y > transform.position.y ? 1 : -1;
        }

        if (Random.value < 0.01f)
        {
            moveSpeedMultiplier = Random.Range(aiMoveSpeedMultiplierMin, aiMoveSpeedMultiplierMax);
        }

        Move(direction);
    }

    private float GetInput()
    {
        return IsLeftPaddle() ? Input.GetAxis(MovePlayer1InputName) : Input.GetAxis(MovePlayer2InputName);
    }

    private void Move(float movement)
    {
        Vector2 velocity = rigidbody2D.linearVelocity;
        velocity.y = moveSpeed * moveSpeedMultiplier * movement;
        rigidbody2D.linearVelocity = velocity;
    }

    public float GetHeight()
    {
        return transform.localScale.y;
    }

    public bool IsLeftPaddle()
    {
        return id == 1;
    }
}