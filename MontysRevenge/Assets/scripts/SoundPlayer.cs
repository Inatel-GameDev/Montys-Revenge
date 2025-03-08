using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    
    public void playSound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void Stop(){
        audioSource.Stop();
    }
}
