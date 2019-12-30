using UnityEngine;

public class Enemy : CharBase
{

    private void Start()
    {
        TargetTile = TileManager.Instance.RandomPoint();
    }

    protected override void ReachedTarget()
    {
        TargetTile = TileManager.Instance.RandomPoint();
    }

    void Update()
    {
        base.Update();
    }
}
