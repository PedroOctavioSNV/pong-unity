using System.Collections;
using UnityEngine;

/// <summary>
/// Controla o efeito de tremor (shake) na posi��o local do GameObject.
/// Utilizado para feedback visual, como impacto ou eventos de a��o.
/// </summary>
public class Shake : MonoBehaviour
{
    // Armazena a posi��o local original para restaurar ap�s o shake
    Vector3 initialPosition;

    void Awake()
    {
        // Guarda a posi��o local inicial do objeto para reset ap�s o shake
        initialPosition = transform.localPosition;
    }

    /// <summary>
    /// Inicia o efeito de shake com deslocamento m�ximo e dura��o especificados.
    /// Se j� houver um shake em andamento, ele ser� interrompido antes de iniciar o novo.
    /// </summary>
    /// <param name="offset">Deslocamento m�ximo em unidades para o shake</param>
    /// <param name="duration">Dura��o total do shake em segundos</param>
    public void StartShake(float offset, float duration)
    {
        StopShake(); // Para shakes existentes para evitar sobreposi��o
        StartCoroutine(ShakeSequence(offset, duration));
    }

    /// <summary>
    /// Para todos os shakes em execu��o e retorna o objeto para a posi��o original.
    /// </summary>
    public void StopShake()
    {
        StopAllCoroutines();
        transform.localPosition = initialPosition;
    }

    /// <summary>
    /// Coroutine que aplica deslocamentos aleat�rios ao transform.localPosition
    /// durante a dura��o especificada para criar o efeito de shake.
    /// </summary>
    /// <param name="offset">Deslocamento m�ximo do shake</param>
    /// <param name="duration">Tempo total do shake</param>
    IEnumerator ShakeSequence(float offset, float duration)
    {
        float durationPassed = 0f;

        while (durationPassed < duration)
        {
            DoShake(offset);
            durationPassed += Time.deltaTime;
            yield return null;
        }

        // Retorna � posi��o original ap�s o fim do shake
        transform.localPosition = initialPosition;
    }

    /// <summary>
    /// Aplica um deslocamento aleat�rio dentro do intervalo [-maxOffset, maxOffset]
    /// na posi��o local do transform para simular o tremor.
    /// </summary>
    /// <param name="maxOffset">Valor m�ximo do deslocamento em cada eixo</param>
    void DoShake(float maxOffset)
    {
        float xOffset = Random.Range(-maxOffset, maxOffset);
        float yOffset = Random.Range(-maxOffset, maxOffset);
        transform.localPosition = initialPosition + new Vector3(xOffset, yOffset, 0f);
    }
}