using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip bounceSFX;
    public AudioClip shatterSFX;
    public AudioClip explodeSFX;
    
    public GameObject pauseMenuUI; // Assign the PauseMenu Panel here
    private bool isPaused = false;

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
        Debug.Log("Quit requested via Input System.");
        
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}