using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Button[] levelButtons;

    void Start()
    {
        // Unlock levels player has completed
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt("LevelUnlocked_" + (i + 1), 0) == 1 || i == 0;
            levelButtons[i].interactable = isUnlocked;

            // SAFE WAY: Find child only if it exists
            Transform lockIcon = levelButtons[i].transform.Find("LockIcon");
            if (lockIcon != null)
            {
                lockIcon.gameObject.SetActive(!isUnlocked);
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level_" + levelNumber);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}