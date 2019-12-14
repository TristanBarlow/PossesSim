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
    public float tileSize = 0.5f;
    private List<Tile> tiles = new List<Tile>();

    private CanWalk canWalk;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(string.Format("{0}, {1}, {2}", nearest(tileSize, 0.7f), nearest(tileSize, .4f), nearest(tileSize, 0.1f)));
        Instance = this;

        canWalk = (int x, int y) =>
        {
            if (x < 0 || x > width || y < 0 || y > height) return false;
            return getTile(x, y).canWalk;
        };

        var xSize = width / 2;
        var ySize = height / 2;
        for (var x = -xSize; x < xSize; x++)
            for (var y = -ySize; y < ySize; y++)
            {
                var clone = Instantiate(tilePrefab, new Vector3(x * tileSize, y * tileSize, 0), new Quaternion());
                clone.transform.SetParent(transform);

                var tile = clone.GetComponent<Tile>();
                tile.SetSprite(randSprite());
                tile.Pos = new Vector2(x + xSize, y + ySize);

                if(Random.Range(0f, 1f) > 0.75)
                {
                    tile.Block();
                }
                tiles.Add(tile);
            }

      
    }

    Tile getTile(int x, int y)
    {
        return this.tiles[((x * width) + y)]; 
    }

    Sprite randSprite(){
        return tileSprites[Random.Range(0, tileSprites.Length)];
     }

    List<Node> getPath(int x1, int y1, int x2, int y2)
    {
        var nodes = AStar.GetPath(x1, y1, x2, y2, canWalk);

        if (nodes == null)
        {
            Debug.LogError("No path found");
        }
        return nodes;
    }

    public Vector2Int worldToCoords(Vector3 world)
    {
        var pos = new Vector2Int(0, 0);
        var foo = nearest(world.x, tileSize) / tileSize;
        pos.x = (int)(foo) + width / 2;
        pos.y = (int)(nearest(world.y, tileSize) / tileSize) + height / 2;

        return pos;
    }

    public static float nearest(float n, float x)
    {
        return Mathf.Round(n / x) * x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
