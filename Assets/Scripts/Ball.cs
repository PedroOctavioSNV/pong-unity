using UnityEngine;

public class Ball : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public float maxInitialAngle = 0.67f;
    public float moveSpeed = 1.0f;

    private void Start()
    {
        InitialPush();
    }

    private void InitialPush()
    {
        Vector2 direction = Vector2.left;
        direction.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rigidbody2D.linearVelocity = direction * moveSpeed;
    }
}