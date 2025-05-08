using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelJson;  // Gán JSON file ở Inspector
    public GameObject boxNormalPrefab;
    public float chieungang = 1f;  // Khoảng cách ngang giữa các ô
    public float chieudoc = 1f;    // Khoảng cách dọc giữa các ô

    public Vector2 spawnPosition = new Vector2(0, 0);

    private LevelData levelData;
    private List<GameObject> spawnedBoxes = new List<GameObject>();

    void Start()
    {
        LoadLevelFromJson();
        InstantiateBoxes();
    }

    void LoadLevelFromJson()
    {
        levelData = JsonUtility.FromJson<LevelData>(levelJson.text);
    }

    void InstantiateBoxes()
    {

        
        foreach (BoxData box in levelData.boxes)
        {
            Vector2 pos = new Vector2(
                spawnPosition.x + (box.column - (levelData.gridSize.columns / 2f)) * chieungang,
                spawnPosition.y - box.row * chieudoc
            );

            if (box.type == "normal")
            {
                GameObject go = Instantiate(boxNormalPrefab, pos, Quaternion.identity);
                go.GetComponent<Box>().SetHP(box.hp);
                spawnedBoxes.Add(go);
            }
            
        }
    }

   
}
