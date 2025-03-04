using UnityEngine;

/// <summary>
/// represents a single sound that can be played
/// it will be keyed by the soundName in the audio player's sound library
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    public string soundName; // this acts as a key to find and play this specific sound from the AudioPlayer script
    public AudioSource audioSource; // this is a required component and contains a reference to the sound to be played 
    
    public void PlaySound()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.Play();
    }
    
    public void StopSound()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        audioSource.Stop();
    }
}
