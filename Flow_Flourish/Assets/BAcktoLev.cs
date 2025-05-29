using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToLevelSelect : MonoBehaviour
{
    public void QuitLevel()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}