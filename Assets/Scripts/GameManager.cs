using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip bounceSFX;
    public AudioClip shatterSFX;
    
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