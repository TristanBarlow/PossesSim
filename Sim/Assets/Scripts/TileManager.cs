using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Sprite[] tileSprites;
    public GameObject tilePrefab;
    public int size = 20;
    private List<Tile> tiles = new List<Tile>();
    // Start is called before the first frame update
    void Start()
    {
        for (var x = -size; x < size; x++)
            for (var y = -size; y < size; y++)
            {
                var clone = Instantiate(tilePrefab, new Vector3(x, y, 0), new Quaternion());
                clone.transform.SetParent(transform);

                var tile = clone.GetComponent<Tile>();
                tile.SetSprite(RandSprite());

                if(Random.Range(0f, 1f) > 0.95)
                {
                    tile.Block();
                }
                tiles.Add(tile);
            }
    }

    Sprite RandSprite(){
        return tileSprites[Random.Range(0, tileSprites.Length)];
     }

    // Update is called once per frame
    void Update()
    {
        
    }
}
