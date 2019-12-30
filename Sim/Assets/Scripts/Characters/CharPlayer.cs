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

    public new void Update()
    {
        base.Update();
        if (!TargetCharacter)
        {
            var t = EnemyManager.Instance.GetNearestEnemy(this);
            if (t)
                TargetCharacter = t;
        }
    }

    protected override void ReachedTarget()
    {
        base.ReachedTarget();
        Debug.Log(TargetTile);
        Debug.Log(currentTile);
        Debug.Log("Reached target");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var e = collision.gameObject.GetComponent<Enemy>();
        if (e)
        {
            EnemyManager.Instance.KillChar(e.name);
            if (e == TargetCharacter)
                TargetCharacter = null;
        }
    }
}
