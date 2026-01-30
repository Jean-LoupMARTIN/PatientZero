using UnityEngine;

public class HelicoHelice : MonoBehaviour
{
    [SerializeField] float rotSpeed = 100;
    // [SerializeField] SpriteRenderer spriteRenderer;
    // [SerializeField] Vector2 alphaRange = new Vector2(0.2f, 0.3f);

    void Update()
    {
        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);

        // Color c = spriteRenderer.color;
        // c.a = Mathf.Lerp(alphaRange.x, alphaRange.y, Random.value);
        // spriteRenderer.color = c;
    }
}
