using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneLoader : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}