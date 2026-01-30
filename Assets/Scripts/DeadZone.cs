using System.Linq;
using UnityEngine;


public class DeadZone : Singleton<DeadZone>
{
    [SerializeField, Min(0.1f)] float spawnRectMarge = 2;

    Vector2 rect;

    void OnDrawGizmosSelected()
    {
        DrawRect();
    }

    void DrawRect()
    {
        if (!TryGetComponent(out LevelsManager levelsManager))
            return;

        Vector2 rect = levelsManager.spawnRect + Vector2.one * spawnRectMarge * 2;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, rect);
    }

    protected override void Awake()
    {
        base.Awake();
        rect = GetComponent<LevelsManager>().spawnRect + Vector2.one * spawnRectMarge * 2;
    }

    void Update()
    {
        DestroyAllTargetsOutRect();
    }

    bool IsInRect(Vector3 point)
    {
        float left = transform.position.x - rect.x / 2f;
        float down = transform.position.y - rect.y / 2f;
        float right = left + rect.x;
        float up = down + rect.y;

        return point.x >= left &&
               point.x < right && 
               point.y > down && 
               point.y < up;
    }


    void DestroyAllTargetsOutRect()
    {
        foreach (DeadZoneTarget target in DeadZoneTarget.set.Where((target) => !IsInRect(target.transform.position)))
            Destroy(target.gameObject);
    }
}
