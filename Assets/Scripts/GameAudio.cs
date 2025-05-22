using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip scoreSound;

    public void PlayScoreSound()
    {
        audioSource.PlayOneShot(scoreSound);
    }
}