using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // Stores the original local position to reset after shaking
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    // Starts a shake effect with given max offset and duration.
    // Stops any existing shake coroutine before starting a new one.
    public void StartShake(float offset, float duration)
    {
        StopShake();
        StartCoroutine(ShakeSequence(offset, duration));
    }

    public void StopShake()
    {
        StopAllCoroutines();
        transform.localPosition = initialPosition;
    }

    // Coroutine that applies shake offsets over the given duration
    private IEnumerator ShakeSequence(float offset, float duration)
    {
        float durationPassed = 0f;
        while (durationPassed < duration)
        {
            DoShake(offset);
            durationPassed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }

    private void DoShake(float maxOffset)
    {
        float xOffset = Random.Range(-maxOffset, maxOffset);
        float yOffset = Random.Range(-maxOffset, maxOffset);
        transform.localPosition = initialPosition + new Vector3(xOffset, yOffset, 0f);
    }
}