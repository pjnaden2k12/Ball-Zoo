using UnityEngine;
public enum BallType
{
    Normal,
    Fire,
    Ice,
    Electric
}

public class Character : MonoBehaviour
{
    public string characterName;
    public BallType ballType; // Loại bóng của nhân vật
    public Sprite characterSprite; // Hình ảnh đại diện của nhân vật
}