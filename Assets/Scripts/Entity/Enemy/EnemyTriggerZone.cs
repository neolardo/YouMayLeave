using System;
using UnityEngine;

[Serializable]
public class EnemyTriggerZone
{
    [Range(0.5f, 15)]
    public float range;
    [Range(30, 360)]
    public float apexAngleDeg;
    [Range(-180, 180)]
    public float rotation;

    private const float angleResolutionDeg = 30;

    public Vector2[] GetCorners( Vector2 lookAt)
    {
        lookAt = lookAt.normalized;
        int numCorners = 2 + Mathf.CeilToInt(apexAngleDeg / angleResolutionDeg);
        var corners = new Vector2[numCorners];
        corners[0] = Vector2.zero;
        corners[1] = Quaternion.AngleAxis(-apexAngleDeg/2f + rotation, Vector3.forward) * lookAt * range;
        for(int i = 2; i<numCorners-1; i++)
        {
            corners[i] = Quaternion.AngleAxis(angleResolutionDeg, Vector3.forward) * corners[i-1];
        }
        corners[numCorners-1] = Quaternion.AngleAxis(apexAngleDeg / 2f + rotation, Vector3.forward) * lookAt * range;
        return corners;
    }
}
