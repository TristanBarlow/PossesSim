using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharBaseManager<Enemy>
{
    public static EnemyManager Instance;

    protected override string NamePref => "Enemy";
    private void Awake()
    {
        Instance = this;    
    }

    public override bool KillChar(string name)
    {
        SpawnChar();
        return base.KillChar(name);
    }

    public Enemy GetNearestEnemy(CharBase c)
    {
        Enemy e = null;
        float closest = Mathf.Infinity;
        foreach(var en in chars)
        {
            var m = (en.Value.transform.position - c.transform.position).magnitude;
            if (!e ||  m< closest)
            {
                closest = m;
                e = en.Value;
            }
        }

        return e;
    }
}
