using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBase : MonoBehaviour
{

    private AStarPath path;
    private bool respectOccupants = false;

    private Vector2Int _tileTarg;
    private CharBase _charTarg;
    
    protected CharBase TargetCharacter {
        get => _charTarg;
        set => _charTarg = value;
    }

    protected Vector3 targetPos = new Vector3();
    protected Vector2Int TargetTile
    {
        set
        {
            _tileTarg = value; RecalculatePath(); respectOccupants = true;
        }
        get
        {
            if(TargetCharacter != null)
            {
                return TileManager.Instance.WorldToTile(_charTarg.transform.position);
            }
            return _tileTarg;
        }
    }

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
    public float size = .5f;

    protected virtual void ReachedTarget() { }
    protected virtual void ReachedNode() { }
    public Tile Tile => currentTile;

    public void Spawned(Tile tile)
    {
        currentTile = tile;
        var success = tile.TryOccupy(this);
        if (!success)
        {
            Debug.LogError("spawned on bad tile");
        }
    }

    public bool InDeadLock(CharBase c)
    {
        return c.currentTile == nextTile && c.nextTile == currentTile;
    }

    public void CheckAndResolveDeadlock(CharBase c)
    {
        if (c.intimidation > intimidation && InDeadLock(c))
        {
            Debug.Log("Deadlock");
            RecalculatePath();
        }
    }

    public bool RecalculatePath(bool withChar = true)
    {
        var p = TileManager.Instance.GetPath(transform.position, TargetTile.x, TargetTile.y, withChar);
       if (!p.IsEmpty)
        {
            path = p;
            return true;
        }

        Debug.Log("No path");
        return false;
    }

    public bool InRange(CharBase c)
    {
        return (c.transform.position - transform.position).magnitude < 0.3;
    }

    public void Update()
    {
        TryMove();
    }

    private bool AtTarget()
    {
        if (!nextTile) return true;
        return Vector3.Distance(transform.position, targetPos) < 0.3f;
    }

    private bool NextNode()
    {
        if (TargetCharacter != null)
        {
            RecalculatePath(respectOccupants);
        }

        if (path == null) return false;

        if (!path.AtEnd)
        {
            var node = path.Next();
            nextTile = TileManager.Instance.GetTile(node.x, node.y);
            var pos = nextTile.transform.position;
            targetPos = new Vector3(pos.x + Random.Range(-1f, 1f) * .05f, pos.y + Random.Range(-1f, 1f) * .05f);
            ReachedNode();
            return true;
        }

        if (nextTile)
        {
            nextTile = null;
            if(TargetCharacter != null  && !InRange(TargetCharacter))
            {
                respectOccupants = false;
                Debug.Log("Not In range");
                return false;
            }
            ReachedTarget();
            Debug.Log(currentTile);
        }

        return false;
    }

    private void Awake()
    {
        intimidation = Random.Range(0f, 1f);
        sprite = GetComponent<SpriteRenderer>();
        var s = TileManager.Instance.tileSize * size;
        transform.localScale = new Vector3(size / sprite.size.x, size / sprite.size.y);
        collider = GetComponent<BoxCollider2D>();

        collider.size = sprite.size * 0.75f;
    }

    private bool TryOccupyTarget => nextTile.TryOccupy(this);

    private void TryMove()
    {
        if (!AtTarget())
        {
            if (!nextTile.IsOccupant(this))
            {
                var success = TryOccupyTarget;
                if (!success && respectOccupants)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
