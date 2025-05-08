using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private BallShooter ballShooter;
    private bool hasHitBottom = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Initialize reference to BallShooter
    public void Initialize(BallShooter shooter)
    {
        ballShooter = shooter;
    }

    public void Launch(Vector2 velocity)
    {
        rb.isKinematic = false;
        rb.linearVelocity = velocity;
        hasHitBottom = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasHitBottom && collision.collider.CompareTag("Bottom"))
        {
            hasHitBottom = true;
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
            ballShooter?.OnBallHitBottom(this);
        }
    }
}