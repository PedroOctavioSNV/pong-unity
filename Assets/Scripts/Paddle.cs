using UnityEngine;

public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public int id;
    public float moveSpeed = 2f;
    public float aiDeadzone = 1f;

    private Vector3 startPosition;
    private int direction = 0;
    private float moveSpeedMultiplier = 1f;

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
            float movement = ProcessInput();
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
            moveSpeedMultiplier = Random.Range(0.05f, 1.5f);
        }

        Move(direction);
    }

    private float ProcessInput()
    {
        float movement = 0f;

        switch (id)
        {
            case 1:
                movement = Input.GetAxis("MovePlayer1");
                break;
            case 2:
                movement = Input.GetAxis("MovePlayer2");
                break;
        }

        return movement;
    }

    private void Move(float movement)
    {
        Vector2 velocity = rigidbody2D.linearVelocity;
        velocity.y = moveSpeed * moveSpeedMultiplier * movement;
        rigidbody2D.linearVelocity = velocity;
    }
}