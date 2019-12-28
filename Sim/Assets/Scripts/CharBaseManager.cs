using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBaseManager<T>: MonoBehaviour where T: CharBase
{
    protected Dictionary<string, T> chars = new Dictionary<string, T>();
    protected int index = 0;
    public GameObject basePrefab;

    public bool KillChar(string name)
    {
        if (!chars.ContainsKey(name)) return false;

        Destroy(chars[name].gameObject);
        chars.Remove(name);
        return true;
    }

    public void SpawnChar()
    {
        var tile = TileManager.Instance.RandomFreeTile();
        var clone = Instantiate(basePrefab, tile.transform.position, new Quaternion());
        clone.transform.SetParent(transform);
        var c = clone.GetComponent<T>();
        c.name = "Char: " + index.ToString();
        c.Spawned(tile);
        index++;

        chars.Add(c.name, c);
    }
}
