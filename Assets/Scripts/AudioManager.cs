using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bounceSFX;
    [SerializeField] private AudioClip shatterSFX;
    [SerializeField] private AudioClip explodeSFX;

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