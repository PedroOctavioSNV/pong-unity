using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [Header("Refs")]
    public AudioSource audioSource;
    public AudioClip scoreSound;

    public void PlayScoreSound()
    {
        audioSource.PlayOneShot(scoreSound);
    }
}