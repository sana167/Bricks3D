using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bounceSFX;
    public AudioClip shatterSFX;
    public AudioClip explodeSFX;

    private void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayBounce()
    {
        PlaySFX(bounceSFX);
    }

    public void PlayShatter()
    {
        PlaySFX(shatterSFX);
    }

    public void PlayExplode()
    {
        PlaySFX(explodeSFX);
    }
}