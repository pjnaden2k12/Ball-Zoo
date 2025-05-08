using System.Collections.Generic;
using UnityEngine;

public class AimPreview : MonoBehaviour
{
    public GameObject dotPrefab;
    public int maxBounces = 1;           // Số lần phản xạ tối đa
    public int dotsPerBounce = 10;        // Số dot mỗi đoạn
    public float dotSpacing = 0.3f;

    private List<GameObject> dots = new List<GameObject>();
    private int totalDots => maxBounces * dotsPerBounce;

    public LayerMask bounceMask; // Layer tường, gạch, v.v.

    private void Start()
    {
        for (int i = 0; i < totalDots; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot);
        }
    }

    public void ShowDots(Vector3 startPos, Vector3 direction)
    {
        Vector3 currentPos = startPos;
        Vector3 currentDir = direction.normalized;
        int dotIndex = 0;

        for (int bounce = 0; bounce < maxBounces; bounce++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, dotsPerBounce * dotSpacing, bounceMask);

            float segmentLength = (hit.collider != null) ? hit.distance : dotsPerBounce * dotSpacing;
            Vector3 endPos = currentPos + currentDir * segmentLength;

            // Vẽ dot trên đoạn này
            for (int i = 0; i < dotsPerBounce && dotIndex < dots.Count; i++)
            {
                float t = (float)i / dotsPerBounce;
                Vector3 pos = Vector3.Lerp(currentPos, endPos, t);
                dots[dotIndex].transform.position = pos;
                dots[dotIndex].SetActive(true);
                dotIndex++;
            }

            if (hit.collider != null)
            {
                currentPos = hit.point + hit.normal * 0.01f; // offset nhỏ tránh bị "kẹt"
                currentDir = Vector2.Reflect(currentDir, hit.normal).normalized;
            }
            else
            {
                break;
            }
        }

        // Ẩn dot dư
        for (int i = dotIndex; i < dots.Count; i++)
            dots[i].SetActive(false);
    }

    public void HideDots()
    {
        foreach (var dot in dots)
        {
            dot.SetActive(false);
        }
    }
}
