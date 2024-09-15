using UnityEngine;

public class DebugHelpers : MonoBehaviour
{
    public static void DrawCell(Vector3 topLeftPos, Color c, float cellSize = 1, float duration = 3f)
    {
        Debug.DrawLine(topLeftPos , topLeftPos + new Vector3(cellSize, 0, 0), c, duration);
        Debug.DrawLine(topLeftPos , topLeftPos + new Vector3(0, -cellSize, 0), c, duration);
        Debug.DrawLine(topLeftPos + new Vector3(cellSize, -cellSize, 0) , topLeftPos + new Vector3(cellSize, 0,0), c, duration);
        Debug.DrawLine(topLeftPos + new Vector3(cellSize, -cellSize, 0) , topLeftPos + new Vector3(0, -cellSize,0), c, duration);
    }
}
