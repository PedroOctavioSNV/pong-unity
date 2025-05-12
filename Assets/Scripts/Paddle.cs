using UnityEngine;

public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public int id;
    public float moveSpeed = 2f;

    public void Update()
    {
        float movement = ProcessInput();
        Move(movement);
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
        Vector2 velocity = rigidbody2D.linearVelocity
    ;   velocity.y = moveSpeed * movement;
        rigidbody2D.linearVelocity = velocity;
    }
}