using System.Collections.Generic;
using UnityEngine;

public class DeadZoneTarget : MonoBehaviour
{
    public static HashSet<DeadZoneTarget> set = new();

    void Awake()
    {
        set.Add(this);
    }

    void OnDestroy()
    {
        set.Remove(this);
    }
}
