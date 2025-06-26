using UnityEngine;

/// <summary>
/// Representa uma zona de pontuação no jogo.
/// Cada zona possui um identificador único (id),
/// utilizado para determinar qual jogador marcou ponto ao colidir com a bola.
/// </summary>
public class ScoreZone : MonoBehaviour
{
    /// <summary>
    /// Identificador da zona de pontuação.
    /// Exemplo: 1 para jogador 1, 2 para jogador 2.
    /// </summary>
    public int id;
}