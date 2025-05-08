using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private BallShooter ballShooter;
    private bool hasHitBottom = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Khởi tạo tham chiếu tới BallShooter
    public void Initialize(BallShooter shooter)
    {
        ballShooter = shooter;
    }

    public void Launch(Vector2 velocity)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = velocity; // sửa lại từ linearVelocity → velocity
        hasHitBottom = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasHitBottom && collision.collider.CompareTag("Bottom"))
        {
            hasHitBottom = true;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            ballShooter?.OnBallHitBottom(this);
        }
    }
}