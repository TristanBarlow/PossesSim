using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBase : MonoBehaviour
{

    Stack<Node> path = new Stack<Node>();
    Tile nextTile;
    Tile currentTile;
    Tile lastTile;

    private Vector2Int _targ;
    private Vector2Int Target { set { _targ = value;  RecalculatePath(); } get { return _targ; } }
    public float speed = 1.0f;
    public bool wait = false;
    public string id = "none";
    public float intimidation = 0.0f;

    private void Awake()
    {
        intimidation = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        TryMove();
    }

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
        return Vector3.Distance(transform.position, nextTile.transform.position) < 0.001f;
    }

    private bool TryOccupyTarget()
    {
        return nextTile.TryOccupy(this);
    }

    private void TryMove()
    {
        if (!AtTarget())
        {
            if (!nextTile.IsOccupant(this))
            {
                var success = TryOccupyTarget();
                if (!success)
                {
                    Debug.Log("failed to occupy tile");
                    return;
                }
                else
                {
                    currentTile.TryRelease(this);
                    currentTile = nextTile;
                }
            }

            if (wait) return;

            float step = speed * Time.deltaTime;
            var target = nextTile.transform;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

          
            return;
        }

     
        if(!NextNode())
             Target = TileManager.Instance.RandomPoint();
    }

    private bool NextNode()
    {
        if (path != null && path.Count > 0)
        {
            var node = path.Pop();
            nextTile = TileManager.Instance.GetTile(node.x, node.y);
            return true;
        }
        nextTile = null;
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

    public void  RecalculatePath()
    {
        path = TileManager.Instance.GetPath(transform.position, Target.x, Target.y);
        NextNode();
        Debug.Log("Calculating path");
    }
}
