using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InputsDisplay : Singleton<InputsDisplay>
{
    [SerializeField] float fadeTime = 1;
    SpriteRenderer spriteRenderer;
    TMP_Text tmpText;

    bool fadeStarted = false;

    public bool FadeStarted => fadeStarted;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tmpText = GetComponentInChildren<TMP_Text>();
    }

    public void StartFade()
    {
        if (fadeStarted)
            return;

        fadeStarted = true;
        StartCoroutine(FadeThanDestroy(fadeTime));
    }

    IEnumerator FadeThanDestroy(float time)
    {
        float startAlpha = spriteRenderer.color.a;
        float t = 0;

        while (t < time)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            float progress = t / time;
            float alpha = Mathf.Lerp(startAlpha, 0, progress);
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
            tmpText.color = c;
        }

        Destroy(gameObject);
    }
}
