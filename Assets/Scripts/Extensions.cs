using UnityEngine;
using UnityEngine.Tilemaps;

public static class Extensions
{
    public static Vector2 TopLeftCornerOfTileAtWorldPos(this Tilemap tilemap, Vector2 position)
    {
        var tile = tilemap.WorldToCell(position);
        var topLeft =  tilemap.CellToWorld(tile);
        topLeft += tilemap.cellSize.y * Vector3.up;
        return topLeft;
    }
}
