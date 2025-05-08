using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelJson;  // gán JSON file ở Inspector
    public GameObject boxNormalPrefab;
    public float chieungang = 0f;
    public float chieudoc = 0f;// khoảng cách giữa các ô

    public Vector2 spawnPosition = new Vector2(0, 0);
    void Start()
    {
        LoadLevel();
    }

    void LoadLevel()
    {
        LevelData data = JsonUtility.FromJson<LevelData>(levelJson.text);

        foreach (BoxData box in data.boxes)
        {
            Vector2 pos = new Vector2(
                spawnPosition.x + (box.column - data.gridSize.columns / 2f) * chieungang,
                spawnPosition.y - (box.row) * chieudoc
            );

            if (box.type == "normal")
            {
                GameObject go = Instantiate(boxNormalPrefab, pos, Quaternion.identity);
                go.GetComponent<Box>().SetHP(box.hp);
            }

            // sau này bạn xử lý thêm các loại khác ở đây
        }
    }
}