using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tilemap Part", menuName = "Tilemap Part", order = 1)]
public class TilemapPart : ScriptableObject
{
    public Tilemap sourceGround;
    public Tilemap sourceWall;
    public int width;
    public int height;

    public void AddToTarget(Tilemap targetGround, Tilemap targetWall, int offsetX, int offsetY)
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++) 
            {
                var groundTile = sourceGround.GetTile(new Vector3Int(x, y));
                var wallTile = sourceWall.GetTile(new Vector3Int(x, y));
                targetGround.SetTile(new Vector3Int(x + offsetX, y + offsetY), groundTile);
                targetWall.SetTile(new Vector3Int(x + offsetX, y + offsetY), wallTile);
            }
        }
        targetGround.RefreshAllTiles();
        targetWall.RefreshAllTiles();
    }
}
