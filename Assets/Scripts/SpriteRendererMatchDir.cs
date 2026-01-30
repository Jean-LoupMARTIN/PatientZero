using UnityEngine;

public class SpriteRendererMatchDir : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] bool onStart = true;
    [SerializeField] bool onLateUpdate = true;
    [SerializeField] Sprite downSprite, upSprite, leftSprite, rightSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (onStart)
            UpdateSpriteRenderers();
    }

    void LateUpdate()
    {
        if (onLateUpdate)
            UpdateSpriteRenderers();
    }

    void UpdateSpriteRenderers()
    {
        Vector2 dir = Vector2.zero;

        if (transform.TryGetComponentInParent(out PlayerMovement pm)) 
            dir = pm.LookDir;

        else if (transform.TryGetComponentInParent(out HumanMovement hm)) 
            dir = hm.MoveDir;

        else if (transform.TryGetComponentInParent(out PoliceManMovement ppm)) 
            dir = ppm.MoveDir;

        if (dir == Vector2.zero)
            return;
            
        float angle = Vector2.Angle(Vector2.up, dir);

        if      (angle < 45)  spriteRenderer.sprite = upSprite;
        else if (angle < 135) spriteRenderer.sprite = dir.x < 0 ? leftSprite : rightSprite;
        else                  spriteRenderer.sprite = downSprite;
    }
}
