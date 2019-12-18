using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharBase
{
    void Hit()
    {
        Debug.Log("Hit");
    }

    void Update()
    {
        base.Update();
    }
}
