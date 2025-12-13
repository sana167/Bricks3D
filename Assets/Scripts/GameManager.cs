using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip bounceSFX;
    public AudioClip shatterSFX;
    void Update()
    {
        // Check if the Escape key is pressed down
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
        Debug.Log("Game exit requested via Escape key.");
        
        // This is the command to close a built application (.exe, .app, etc.)
        Application.Quit();

        // The following code allows you to stop the game instantly 
        // when running in the Unity Editor for quick testing.
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