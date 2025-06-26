using System.Collections;
using UnityEngine;

/// <summary>
/// Controla o efeito de tremor (shake) na posição local do GameObject.
/// Utilizado para feedback visual, como impacto ou eventos de ação.
/// </summary>
public class Shake : MonoBehaviour
{
    // Armazena a posição local original para restaurar após o shake
    Vector3 initialPosition;

    void Awake()
    {
        // Guarda a posição local inicial do objeto para reset após o shake
        initialPosition = transform.localPosition;
    }

    /// <summary>
    /// Inicia o efeito de shake com deslocamento máximo e duração especificados.
    /// Se já houver um shake em andamento, ele será interrompido antes de iniciar o novo.
    /// </summary>
    /// <param name="offset">Deslocamento máximo em unidades para o shake</param>
    /// <param name="duration">Duração total do shake em segundos</param>
    public void StartShake(float offset, float duration)
    {
        StopShake(); // Para shakes existentes para evitar sobreposição
        StartCoroutine(ShakeSequence(offset, duration));
    }

    /// <summary>
    /// Para todos os shakes em execução e retorna o objeto para a posição original.
    /// </summary>
    public void StopShake()
    {
        StopAllCoroutines();
        transform.localPosition = initialPosition;
    }

    /// <summary>
    /// Coroutine que aplica deslocamentos aleatórios ao transform.localPosition
    /// durante a duração especificada para criar o efeito de shake.
    /// </summary>
    /// <param name="offset">Deslocamento máximo do shake</param>
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

        // Retorna à posição original após o fim do shake
        transform.localPosition = initialPosition;
    }

    /// <summary>
    /// Aplica um deslocamento aleatório dentro do intervalo [-maxOffset, maxOffset]
    /// na posição local do transform para simular o tremor.
    /// </summary>
    /// <param name="maxOffset">Valor máximo do deslocamento em cada eixo</param>
    void DoShake(float maxOffset)
    {
        float xOffset = Random.Range(-maxOffset, maxOffset);
        float yOffset = Random.Range(-maxOffset, maxOffset);
        transform.localPosition = initialPosition + new Vector3(xOffset, yOffset, 0f);
    }
}