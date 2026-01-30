using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] Vector2 scaleRange = new Vector2(0.2f, 0.4f);
    [SerializeField] float timeToTarget = 1;
    [SerializeField] AnimationCurve curveToTarget;
    [SerializeField] Gradient gradient;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Awake()
    {
        transform.localScale = Vector3.one * Random.Range(scaleRange.x, scaleRange.y);
    }
    public void StartGoToTarget(Vector3 targetPosition)
    {
        StartCoroutine(GoToTarget(targetPosition, timeToTarget));
    }

    IEnumerator GoToTarget(Vector3 targetPosition, float timeToTarget)
    {
        Vector3 startPosition = transform.position;

        float t = 0;

        while (t < timeToTarget)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            float progress = t / timeToTarget;
            transform.position = Vector3.Lerp(startPosition, targetPosition, curveToTarget.Evaluate(progress));
            spriteRenderer.color = gradient.Evaluate(progress);
        }

        Destroy(gameObject);
    }
}
