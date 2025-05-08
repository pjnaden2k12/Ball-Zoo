using UnityEngine;
using TMPro;

public class Box : MonoBehaviour
{
    public int hp = 1;
    public TextMesh text; // gán text con hiển thị HP (nếu có)

    public void SetHP(int value)
    {
        hp = value;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (text != null)
            text.text = hp.ToString();
    }
}