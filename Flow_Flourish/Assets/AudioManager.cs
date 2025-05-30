using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip menuMusic;
    public AudioClip levelMusic;
    public AudioClip pomodoroMusic;

    private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayLevelMusic()
    {
        musicSource.clip = levelMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPomodoroMusic()
    {
        musicSource.clip = pomodoroMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
}
