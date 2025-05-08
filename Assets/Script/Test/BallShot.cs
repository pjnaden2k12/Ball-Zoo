using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallShooter : MonoBehaviour
{
    public Ball ballPrefab;
    public Transform shootPoint;
    public int ballCount = 10;
    public float ballSpeed = 10f;
    public float delayBetweenShots = 0.1f;

    public AimPreview aimPreview;

    private List<Ball> activeBalls = new List<Ball>();
    private bool isShooting = false;
    private bool isAiming = false;

    private Vector2 aimDirection;

    void Update()
    {
        if (isShooting) return;

        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;

            aimDirection = (worldPos - shootPoint.position).normalized;
            aimPreview.ShowDots(shootPoint.position, aimDirection);
            isAiming = true;
        }
        else if (isAiming)
        {
            StartCoroutine(ShootBalls(aimDirection));
            aimPreview.HideDots();
            isAiming = false;
        }
    }

    private IEnumerator ShootBalls(Vector3 direction)
    {
        isShooting = true;
        activeBalls.Clear();

        for (int i = 0; i < ballCount; i++)
        {
            Ball ball = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
            ball.Initialize(this);
            ball.Launch(direction * ballSpeed);
            activeBalls.Add(ball);
            yield return new WaitForSeconds(delayBetweenShots);
        }

        // Wait until all balls are collected and destroyed before allowing next shot
        while (activeBalls.Count > 0)
        {
            yield return null;
        }

        isShooting = false;
    }

    public void OnBallHitBottom(Ball ball)
    {
        if (!activeBalls.Contains(ball)) return;

        StartCoroutine(MoveBallToPointAndDestroy(ball, shootPoint.position));
    }

    private IEnumerator MoveBallToPointAndDestroy(Ball ball, Vector3 point)
    {
        activeBalls.Remove(ball);

        float speed = 10f;

        while (Vector3.Distance(ball.transform.position, point) > 0.05f)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, point, speed * Time.deltaTime);
            yield return null;
        }
        ball.transform.position = point;

        Destroy(ball.gameObject);
    }
}

