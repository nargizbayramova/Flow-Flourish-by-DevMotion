using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelButtons; // Assign all buttons in Inspector

    void Start()
    {
        // Lock levels beyond Level 1 (initially)
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i > 0)
                levelButtons[i].interactable = false;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        // Save selected level (we'll use this later in PipeGrid)
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);

        // Load the game scene
        SceneManager.LoadScene("PipeWithAutoGenerateGrid");
    }
}