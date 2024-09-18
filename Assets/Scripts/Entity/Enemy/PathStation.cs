using System;
using UnityEngine;

[Serializable]
public class PathStation
{
    public Vector2 relativePosition;
    public float stopDuration;
    [HideInInspector] public Vector2 savedPosition;

    public void SavePosition(Transform transform)
    {
        savedPosition = (Vector2)transform.position + relativePosition;
    }

}
