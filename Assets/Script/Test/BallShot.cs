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

    // Khoảng cách dịch xuống theo trục Y (căn chỉnh với map bạn)
    public float boxMoveDownDistance = 1f;

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

        // Đợi tất cả ball thu về (danh sách activeBalls rỗng)
        while (activeBalls.Count > 0)
        {
            yield return null;
        }

        // Tất cả ball đã thu lại, dịch các box xuống 1 hàng
        ShiftBoxesDown();

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

    // Hàm dịch tất cả box xuống 1 hàng
    private void ShiftBoxesDown()
    {
        // Tìm tất cả box hiện tại trong scene
        Box[] boxes = Object.FindObjectsByType<Box>(FindObjectsSortMode.None);

        foreach (Box box in boxes)
        {
            Vector3 pos = box.transform.position;
            pos.y -= boxMoveDownDistance;  // Dịch xuống theo trục Y
            box.transform.position = pos;
        }
    }
}
