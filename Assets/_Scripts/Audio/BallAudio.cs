using UnityEngine;

/// <summary>
/// Respons�vel por reproduzir os sons da bola ao colidir com paredes e raquetes.
/// Utiliza AudioSource para tocar efeitos sonoros espec�ficos.
/// </summary>
public class BallAudio : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] AudioSource audioSource;     // Componente respons�vel pela reprodu��o dos sons
    [SerializeField] AudioClip wallSound;         // Som reproduzido ao colidir com uma parede
    [SerializeField] AudioClip paddleSound;       // Som reproduzido ao colidir com uma raquete

    /// <summary>
    /// Toca o som de colis�o com a parede usando PlayOneShot.
    /// Ideal para efeitos curtos e sem sobreposi��o de inst�ncias.
    /// </summary>
    public void PlayWallSound()
    {
        audioSource.PlayOneShot(wallSound);
    }

    /// <summary>
    /// Toca o som de colis�o com uma raquete (paddle) usando PlayOneShot.
    /// Permite sobreposi��o sem cortar sons anteriores.
    /// </summary>
    public void PlayPaddleSound()
    {
        audioSource.PlayOneShot(paddleSound);
    }
}