using UnityEngine;

public class SpriteOrder : MonoBehaviour
{
    [SerializeField] float positionCoef = 1000;

    int offset;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        offset = spriteRenderer.sortingOrder;
    }

    void LateUpdate()
    {
        OrderMatchY();
    }

    void OrderMatchY()
    {
        spriteRenderer.sortingOrder = (int)(-transform.position.y * positionCoef) + offset;
    }
}
