using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private List<TilemapPart> tilemapParts;
    [SerializeField] private Player player;
    private int tilemapHeight;
    private const int initialTilemapHeight = 10;
    private const float maxHeightDifference = 6;

    void Start()
    {
        tilemapHeight = initialTilemapHeight;
    }

    void FixedUpdate()
    {
        float playerPositionY = player.transform.position.y;
        if(tilemapHeight - playerPositionY < maxHeightDifference)
        {
            AddNextTilemapPart();
        }
    }

    private void AddNextTilemapPart()
    {
        var part = tilemapParts[Random.Range(0, tilemapParts.Count)];
        part.AddToTarget(groundTilemap, wallTilemap, 0, tilemapHeight);
        tilemapHeight += part.height;
    }

}
