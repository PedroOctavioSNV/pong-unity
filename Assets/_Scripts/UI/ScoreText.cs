using TMPro;
using UnityEngine;

/// <summary>
/// Controla a exibição do texto de pontuação de um jogador.
/// Atualiza o valor exibido e realiza a animação de destaque.
/// </summary>
public class ScoreText : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] TextMeshProUGUI text;    // Referência ao componente de texto para mostrar a pontuação
    [SerializeField] Animator animator;       // Referência ao Animator para executar animações do texto

    /// <summary>
    /// Atualiza o texto da pontuação para o valor especificado.
    /// </summary>
    /// <param name="value">Valor da pontuação a ser exibida</param>
    public void SetScore(int value)
    {
        text.text = value.ToString();
    }

    /// <summary>
    /// Dispara a animação que destaca o texto da pontuação.
    /// </summary>
    public void Highlight()
    {
        animator.SetTrigger("highlight");
    }
}