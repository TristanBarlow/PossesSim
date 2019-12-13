using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Sprite[] tileSprites;
    public GameObject tilePrefab;
    public int width = 40;
    public int height = 40;
    private List<Tile> tiles = new List<Tile>();
    // Start is called before the first frame update
    void Start()
    {
        var xSize = width / 2;
        var ySize = height / 2;
        for (var x = -xSize; x < xSize; x++)
            for (var y = -ySize; y < ySize; y++)
            {
                var clone = Instantiate(tilePrefab, new Vector3(x, y, 0), new Quaternion());
                clone.transform.SetParent(transform);

                var tile = clone.GetComponent<Tile>();
                tile.SetSprite(RandSprite());
                tile.Pos = new Vector2(x + xSize, y + ySize);

                if(Random.Range(0f, 1f) > 0.75)
                {
                    tile.Block();
                }
                tiles.Add(tile);
            }

        var nodes = AStar.GetPath(0, 0, 20, 20, (int x, int y) =>
        {
            Debug.Log("X:" + x + " Y:" + y);
            if (x < 0 || x > width || y < 0 || y > height) return false;
            return getTile(x, y).canWalk;
        });

        if(nodes == null)
        {
            Debug.LogError("No path found");
        }
        foreach(var node in nodes)
        {
            getTile(node.x, node.y).Path();
        }
    }

    Tile getTile(int x, int y)
    {
        return this.tiles[((x * width) + y)]; 
    }

    Sprite RandSprite(){
        return tileSprites[Random.Range(0, tileSprites.Length)];
     }

    // Update is called once per frame
    void Update()
    {
        
    }

}
