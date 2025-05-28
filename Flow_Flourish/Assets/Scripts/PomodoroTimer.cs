using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SimplePomodoro : MonoBehaviour
{
    // UI References
    public GameObject timerSelectionPanel;
    public GameObject activeTimerPanel;
    public TMP_Text timerText;
    public TMP_Dropdown minutesDropdown;

    private float timeRemaining;
    private bool timerRunning;

    void Start()
    {
        // Initialize UI
        timerSelectionPanel.SetActive(true);
        activeTimerPanel.SetActive(false);

        // Debug current states
        Debug.Log($"Start() - SelectionPanel: {timerSelectionPanel.activeSelf}, ActivePanel: {activeTimerPanel.activeSelf}");
    }

    public void StartTimer()
    {
        // CORRECTED time calculation: 0=5min, 1=10min, 2=15min etc.
        timeRemaining = (minutesDropdown.value + 1) * 5 * 60f;

        Debug.Log($"StartTimer() - Setting time to: {timeRemaining / 60} minutes");

        // Toggle panels
        timerSelectionPanel.SetActive(false);
        activeTimerPanel.SetActive(true);
        timerRunning = true;

        Debug.Log($"Panels - Selection: {timerSelectionPanel.activeSelf}, Active: {activeTimerPanel.activeSelf}");
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        // Update display
        int minutes = (int)(timeRemaining / 60);
        int seconds = (int)(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (timeRemaining <= 0)
        {
            timerRunning = false;
            timerText.text = "Done! 🎉";
            Invoke("ReturnToMenu", 2f);
        }
    }

    public void QuitTimer() => ReturnToMenu();
    void ReturnToMenu() => SceneManager.LoadScene("StartScene");
}