using System;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(LevelManager))]
public class GameManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign the PauseMenu Panel here
    private bool isPaused = false;
    [SerializeField] private BrickLevelSpawner level;
    [SerializeField] private MenuController menuController;

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume time
        isPaused = false;
        
        // Optional: Lock/Hide cursor if playing on PC
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze time
        isPaused = true;

        // Optional: Show cursor to click the Resume button
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    // This is now called by the InputHandler
    public void RequestQuit()
    {
        menuController.ShowQuitDialog();
    }
}