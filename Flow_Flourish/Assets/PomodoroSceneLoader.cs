using UnityEngine;
using UnityEngine.SceneManagement;

public class PomodoroSceneLoader : MonoBehaviour
{
    public void LoadPomodoroScene()
    {
        SceneManager.LoadScene("Pomodoro");
    }
}