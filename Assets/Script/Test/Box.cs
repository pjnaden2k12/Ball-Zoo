using UnityEngine;
using TMPro;

public class Box : MonoBehaviour
{
    public int hp = 1;
    public TextMesh text; // hoặc TextMesh nếu dùng 3D Text

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
public void TakeDamage(float amount)
{
    hp -= Mathf.RoundToInt(amount);
    if (hp <= 0)
    {
        Destroy(gameObject);
    }
    else
    {
        UpdateVisual();
    }
}

}
