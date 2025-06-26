using UnityEngine;

/// <summary>
/// Responsável por reproduzir os sons da bola ao colidir com paredes e raquetes.
/// Utiliza AudioSource para tocar efeitos sonoros específicos.
/// </summary>
public class BallAudio : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] AudioSource audioSource;     // Componente responsável pela reprodução dos sons
    [SerializeField] AudioClip wallSound;         // Som reproduzido ao colidir com uma parede
    [SerializeField] AudioClip paddleSound;       // Som reproduzido ao colidir com uma raquete

    /// <summary>
    /// Toca o som de colisão com a parede usando PlayOneShot.
    /// Ideal para efeitos curtos e sem sobreposição de instâncias.
    /// </summary>
    public void PlayWallSound()
    {
        audioSource.PlayOneShot(wallSound);
    }

    /// <summary>
    /// Toca o som de colisão com uma raquete (paddle) usando PlayOneShot.
    /// Permite sobreposição sem cortar sons anteriores.
    /// </summary>
    public void PlayPaddleSound()
    {
        audioSource.PlayOneShot(paddleSound);
    }
}