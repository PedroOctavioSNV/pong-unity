using UnityEngine;

public class Paddle : MonoBehaviour
{
    [Header("Refs")]
    public new Rigidbody2D rigidbody2D;

    [Header("Configs")]
    [SerializeField]
    private int id;
    public float moveSpeed = 2f;
    private Vector3 startPosition;
    private float moveSpeedMultiplier = 1f;
    private const string MovePlayer1InputName = "MovePlayer1";
    private const string MovePlayer2InputName = "MovePlayer2";

    [Header("AI")]
    public float aiDeadzone = 1f;
    public float aiMoveSpeedMultiplierMin = 0.5f;
    public float aiMoveSpeedMultiplierMax = 1.5f;
    private int direction = 0;

    private void Start()
    {
        startPosition = transform.position;
        // Subscribe ResetPosition to the GameManager reset event
        GameManager.instance.onReset += ResetPosition;
    }

    private void Update()
    {
        // If this is player 2 and the current mode is Player vs AI, use AI movement
        if (id == 2 && GameManager.instance.IsPlayer2Ai())
        {
            MoveAi();
        }
        else
        {
            // Otherwise, get input and move manually
            float movement = GetInput();
            Move(movement);
        }
    }

    private void ResetPosition()
    {
        transform.position = startPosition;
    }

    private float GetInput()
    {
        return IsLeftPaddle() ? Input.GetAxis(MovePlayer1InputName) : Input.GetAxis(MovePlayer2InputName);
    }

    // Determines if this paddle is the left one (Player 1)
    public bool IsLeftPaddle()
    {
        return id == 1;
    }

    private void Move(float movement)
    {
        Vector2 velocity = rigidbody2D.linearVelocity;
        velocity.y = moveSpeed * moveSpeedMultiplier * movement;
        rigidbody2D.linearVelocity = velocity;
    }

    // Handles AI logic for tracking the ball and deciding movement direction and speed
    private void MoveAi()
    {
        Vector2 ballPosition = GameManager.instance.ball.transform.position;

        // Determine direction to move based on ball position and deadzone
        if (Mathf.Abs(ballPosition.y - transform.position.y) > aiDeadzone)
        {
            direction = ballPosition.y > transform.position.y ? 1 : -1;
        }

        // Occasionally randomize the AI's movement speed multiplier
        if (Random.value < 0.01f)
        {
            moveSpeedMultiplier = Random.Range(aiMoveSpeedMultiplierMin, aiMoveSpeedMultiplierMax);
        }

        Move(direction);
    }

    public float GetHeight()
    {
        return transform.localScale.y;
    }
}