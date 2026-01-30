using UnityEngine;

public class AreaOscillation : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] Vector2 aplhaRange = new Vector2(0, 1);
    float offset;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offset = Random.value * 1000;
    }

    void Update()
    {
        Color c = spriteRenderer.color;
        c.a = Mathf.Lerp(aplhaRange.x, aplhaRange.y, Mathf.Cos(offset + Time.time * speed));
        spriteRenderer.color = c;
    }
}
