using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    public Sprite[] tileSprites;
    public GameObject tilePrefab;
    public int width = 40;
    public int height = 40;
    public float tileSize = 1f;

    private List<Tile> tiles = new List<Tile>();
    private List<Tile> clearTiles = new List<Tile>();

    private CanWalk canWalkWithChar;
    private CanWalk canWalkNoChar;


    private void Awake()
    {
        Instance = this;

        canWalkWithChar = (int x, int y) =>
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            return GetTile(x, y).CanWalkWithChar;
        };

        canWalkNoChar = (int x, int y) =>
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return false;
            return GetTile(x, y).CanWalkNoChar;
        };

        var xSize = width / 2;
        var ySize = height / 2;
        for (var x = -xSize; x < xSize; x++)
            for (var y = -ySize; y < ySize; y++)
            {
                var clone = Instantiate(tilePrefab, new Vector3(x * tileSize, y * tileSize, 0), new Quaternion());
                clone.transform.SetParent(transform);
                clone.name = "x:" + (x + width/2) + "y:" + (y + width/2);

                var tile = clone.GetComponent<Tile>();
                tile.SetSprite(RandSprite());
                tile.Pos = new Vector2Int(x + xSize, y + ySize);

                if (Random.Range(0f, 1f) > 0.9)
                {
                    tile.Block();
                }
                else
                {
                    clearTiles.Add(tile);
                }
                tiles.Add(tile);
            }
    }

    public Vector2Int RandomPoint()
    {
        var index = Random.Range(0, this.tiles.Count - 1);
        return this.tiles[index].Pos;
    }

    public Tile RandomFreeTile()
    {
        var index = Random.Range(0, this.clearTiles.Count - 1);
        return this.clearTiles[index];
    }

    public Tile GetTile(int x, int y)
    {
        var index = ((x * width) + y);
        return this.tiles[index]; 
    }

    Sprite RandSprite(){
        return tileSprites[Random.Range(0, tileSprites.Length)];
    }

    public AStarPath GetPath(int x1, int y1, int x2, int y2, bool withChar)
    {
        var result = AStar.GetPath(x1, y1, x2, y2, withChar ? canWalkWithChar : canWalkNoChar);

        if (result.IsEmpty)
        {
            Debug.LogError("No path found");
        }
        return result;
    }

    public AStarPath GetPath(Vector3 strt, Vector3 end, bool withChar = true)
    {
        var strtTile = WorldToTile(strt);
        var endTile = WorldToTile(end);
        return GetPath(strtTile.x, strtTile.y, endTile.x, endTile.y, withChar);
    }

    public AStarPath GetPath(Vector3 strt, int x, int y, bool withChar = true)
    {
        var strtTile = WorldToTile(strt);
        return GetPath(strtTile.x, strtTile.y, x, y, withChar);
    }

    public Vector2Int WorldToTile(Vector3 world)
    {
        var pos = new Vector2Int(0, 0);
        pos.x = (int)(Nearest(world.x, tileSize) / tileSize) + width / 2;
        pos.y = (int)(Nearest(world.y, tileSize) / tileSize) + height / 2;

        return pos;
    }

    public static float Nearest(float n, float x)
    {
        return Mathf.Round(n / x) * x;
    }
}
