using UnityEngine;

public class CamFollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;

    void LateUpdate()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (!target)
            return;

        float z = transform.position.z;
        Vector3 pos = target.position;
        pos.z = z;
        transform.position = pos;
    }
}
