using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttackIndicator : MonoBehaviour
{
    [SerializeField] private Transform fillTransform;


    public void StartExpanding(float duration)
    {
        StartCoroutine(ExpandCoroutine(duration));
    }

    private IEnumerator ExpandCoroutine(float duration)
    {
        float elapsed = 0;
        Vector3 initialScale = fillTransform.localScale;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            fillTransform.localScale = Vector3.Lerp(initialScale, Vector3.one, t);
            yield return null;
        }

        fillTransform.localScale = Vector3.one;
    }

    public void SetRadius(float radius)
    {
        gameObject.transform.localScale = new Vector3(radius, radius, radius);
    }

}
