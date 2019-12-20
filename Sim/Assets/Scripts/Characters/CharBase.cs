﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBase : MonoBehaviour
{

    private Stack<Node> path = new Stack<Node>();
    private Vector2Int _targ;

    protected Vector3 targetPos = new Vector3();
    protected Vector2Int TargetTile { set { _targ = value;  RecalculatePath(); } get => _targ;  }
    protected Tile currentTile;
    protected Tile nextTile;
    protected SpriteRenderer sprite;
    protected BoxCollider2D collider;
    protected string Name => name;

    protected float timeStuck;

    public float maxWaitTime = 2f;
    public float speed = 1.0f;
    public bool wait;
    public float intimidation;

    private void Awake()
    {
        intimidation = Random.Range(0f, 1f);
        sprite = GetComponent<SpriteRenderer>();
        collider  = GetComponent<BoxCollider2D>();

        collider.size = sprite.size * 0.75f;
    }

    public void Update()
    {
        TryMove();
    }

    protected virtual void ReachedTarget() { }
    protected virtual void ReachedNode() { }

    public void Spawned(Tile tile)
    {
        currentTile = tile;
        var success = tile.TryOccupy(this);
        if (!success)
        {
            Debug.LogError("spawned on bad tile");
        }
    }

    bool AtTarget()
    {
        if (!nextTile) return true;
        return Vector3.Distance(transform.position, targetPos) < 0.3f;
    }

    private bool TryOccupyTarget => nextTile.TryOccupy(this);
    

    private void TryMove()
    {
        
        if (!AtTarget())
        {
            if (!nextTile.IsOccupant(this))
            {
                var success = TryOccupyTarget;
                if (!success)
                {
                    timeStuck += Time.deltaTime;
                    if (timeStuck > maxWaitTime)
                    {
                        if (RecalculatePath())
                        {
                            Debug.Log("waited too long recalculating path");
                            timeStuck = 0;
                        }
                        else
                        {
                            Debug.LogError("Cannot find path");
                        }
                    }
                    return;
                }
                timeStuck = 0;
                currentTile?.TryRelease(this);
                currentTile = nextTile;
            }

            if (wait) return;

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            return;
        }

         NextNode();
    }

    private bool NextNode()
    {
        if (path != null && path.Count > 0)
        {
            var node = path.Pop();
            nextTile = TileManager.Instance.GetTile(node.x, node.y);
            var pos = nextTile.transform.position;
            targetPos = new Vector3(pos.x + Random.Range(-1f, 1f) * .05f, pos.y + Random.Range(-1f, 1f) * .05f);
            ReachedNode();
            return true;
        }
        if (path.Count == 0)
        {
            ReachedTarget();
            nextTile = null;
        }
        return false;
    }

    public bool InDeadLock(CharBase c)
    {
        return c.currentTile == nextTile && c.nextTile == currentTile;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var c = collision.GetComponent<CharBase>();
        if (c) CheckAndResolveDeadlock(c);
    }

    public void CheckAndResolveDeadlock(CharBase c)
    {
        if (c.intimidation > intimidation && InDeadLock(c))
        {
            Debug.Log("Deadlock");
            RecalculatePath();
        }
    }

    public bool RecalculatePath()
    {
        var p = TileManager.Instance.GetPath(transform.position, TargetTile.x, TargetTile.y);
       if (p.Count > 0)
        {
            path = p;
            return NextNode();
        }
        Debug.Log("No path");
        return false;
    }
}
