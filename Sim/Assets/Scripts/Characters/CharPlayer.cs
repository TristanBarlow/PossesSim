using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharPlayer : CharBase
{
    // Start is called before the first frame update
    void Start()
    {
        TargetTile = TileManager.Instance.RandomPoint();
    }

    protected override void ReachedTarget()
    {
        base.ReachedTarget();
        Debug.Log("Reached target");
        TargetTile = TileManager.Instance.RandomPoint();
    }
}
