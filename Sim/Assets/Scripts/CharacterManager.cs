using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;
    public GameObject characterBase;

    private int index = 0;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var tile = TileManager.Instance.RandomFreeTile();
            var clone = Instantiate(characterBase, tile.transform.position, new Quaternion());
            clone.transform.SetParent(transform);
            var c = clone.GetComponent<CharBase>();
            c.name = "Char: " + index.ToString();
            c.Spawned(tile);
            index++;
        }
    }
}
