using TMPro;
using UnityEngine;

/// <summary>
/// Controla a exibi��o do texto de pontua��o de um jogador.
/// Atualiza o valor exibido e realiza a anima��o de destaque.
/// </summary>
public class ScoreText : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] TextMeshProUGUI text;    // Refer�ncia ao componente de texto para mostrar a pontua��o
    [SerializeField] Animator animator;       // Refer�ncia ao Animator para executar anima��es do texto

    /// <summary>
    /// Atualiza o texto da pontua��o para o valor especificado.
    /// </summary>
    /// <param name="value">Valor da pontua��o a ser exibida</param>
    public void SetScore(int value)
    {
        text.text = value.ToString();
    }

    /// <summary>
    /// Dispara a anima��o que destaca o texto da pontua��o.
    /// </summary>
    public void Highlight()
    {
        animator.SetTrigger("highlight");
    }
}