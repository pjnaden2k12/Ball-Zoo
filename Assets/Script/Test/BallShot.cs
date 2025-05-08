

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallShooter : MonoBehaviour
{
    public Ball ballPrefab;
    public GameObject shootPointPrefab;
    public int ballCount = 10;
    public float ballSpeed = 10f;
    public float delayBetweenShots = 0.1f;
    public AimPreview aimPreview;

    private List<Ball> activeBalls = new List<Ball>();
    private bool isShooting = false;
    private bool isAiming = false;

    private Vector2 aimDirection;
    private Vector3 currentShootPosition;
    private GameObject currentShootPointObject;

    private void Start()
    {
        // Create initial shoot point GameObject at current transform position
        currentShootPosition = transform.position;
        currentShootPointObject = Instantiate(shootPointPrefab, currentShootPosition, Quaternion.identity);
    }

    void Update()
    {
        if (isShooting) return;

        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;

            aimDirection = (worldPos - currentShootPosition).normalized;
            aimPreview.ShowDots(currentShootPosition, aimDirection);
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
            Ball ball = Instantiate(ballPrefab, currentShootPosition, Quaternion.identity);
            ball.Initialize(this);
            ball.Launch(direction * ballSpeed);
            activeBalls.Add(ball);
            yield return new WaitForSeconds(delayBetweenShots);
        }

        // Wait until all balls are collected before allowing next shot
        while (activeBalls.Count > 0)
        {
            yield return null;
        }

        isShooting = false;
    }

    public void OnBallHitBottom(Ball ball)
    {
        if (!activeBalls.Contains(ball)) return;

        // If it's the first ball hitting bottom this turn, update shoot position and recreate shoot point
        if (currentShootPointObject == null || Vector3.Distance(ball.transform.position, currentShootPosition) > 0.1f)
        {
            if (currentShootPointObject != null)
            {
                Destroy(currentShootPointObject);
            }
            currentShootPosition = ball.transform.position;
            currentShootPointObject = Instantiate(shootPointPrefab, currentShootPosition, Quaternion.identity);
        }

        StartCoroutine(MoveBallToPoint(ball, currentShootPosition));
    }

    private IEnumerator MoveBallToPoint(Ball ball, Vector3 point)
    {
        activeBalls.Remove(ball);

        float speed = 10f;

        while (Vector3.Distance(ball.transform.position, point) > 0.05f)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, point, speed * Time.deltaTime);
            yield return null;
        }
        ball.transform.position = point;
    }
}

