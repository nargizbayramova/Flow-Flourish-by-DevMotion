using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageObjects : MonoBehaviour
{
    public void StartGame()
    {
        // Load Level Selection instead of the game directly
        SceneManager.LoadScene("LevelSelection");
    }
}