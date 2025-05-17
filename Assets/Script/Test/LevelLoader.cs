using UnityEngine;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    public float chieungang = 1f;
    public float chieudoc = 1f;
    public Vector2 spawnPosition = new Vector2(0, 0);

    [System.Serializable]
    public class BoxPrefabEntry
    {
        public string type;
        public GameObject prefab;
    }

    public List<BoxPrefabEntry> boxPrefabs; // khai báo từ Inspector
    private Dictionary<string, GameObject> prefabDict;

    private LevelData levelData;
    private List<GameObject> spawnedBoxes = new List<GameObject>();

    void Start()
    {
        LoadLevelFromJson();
        BuildPrefabDict();
        InstantiateBoxes();
    }

    void LoadLevelFromJson()
    {
        string levelFileName = LevelManager.SelectedLevelName;
        TextAsset json = Resources.Load<TextAsset>(levelFileName);
        if (json == null)
        {
            Debug.LogError("Không tìm thấy file JSON: " + levelFileName);
            return;
        }
        levelData = JsonUtility.FromJson<LevelData>(json.text);
    }

    void BuildPrefabDict()
    {
        prefabDict = new Dictionary<string, GameObject>();
        foreach (var entry in boxPrefabs)
        {
            if (!prefabDict.ContainsKey(entry.type))
            {
                prefabDict.Add(entry.type, entry.prefab);
            }
        }
    }

    void InstantiateBoxes()
    {
        foreach (BoxData box in levelData.boxes)
        {
            Vector2 pos = new Vector2(
                spawnPosition.x + (box.column - (levelData.gridSize.columns / 2f)) * chieungang,
                spawnPosition.y - box.row * chieudoc
            );

            if (prefabDict.TryGetValue(box.type, out GameObject prefab))
            {
                GameObject go = Instantiate(prefab, pos, Quaternion.identity);
                go.GetComponent<Box>().SetHP(box.hp);
                spawnedBoxes.Add(go);
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy prefab cho type: {box.type}");
            }
        }
    }
}
