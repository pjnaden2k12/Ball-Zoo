using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayLevel(string levelName)
    {
        LevelManager.SelectedLevelName = levelName; // gán tên JSON
        SceneManager.LoadScene("GameScene");        // vào scene chơi
    }
}