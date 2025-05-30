using UnityEngine;

public class BallAudio : MonoBehaviour
{
    [Header("Refs")]
    public AudioSource audioSource;
    public AudioClip wallSound;
    public AudioClip paddleSound;

    public void PlayWallSound()
    {
        audioSource.PlayOneShot(wallSound);
    }

    public void PlayPaddleSound()
    {
        audioSource.PlayOneShot(paddleSound);
    }
}