using UnityEngine;

/// <summary>
/// Controla os efeitos sonoros relacionados ao progresso do jogo,
/// como o som tocado ao marcar um ponto.
/// </summary>
public class GameAudio : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] AudioSource audioSource;   // Componente respons�vel por reproduzir os efeitos sonoros
    [SerializeField] AudioClip scoreSound;      // �udio reproduzido quando um jogador marca ponto

    /// <summary>
    /// Reproduz o som de pontua��o usando PlayOneShot.
    /// Ideal para efeitos r�pidos sem interromper sons em andamento.
    /// </summary>
    public void PlayScoreSound()
    {
        audioSource.PlayOneShot(scoreSound);
    }
}