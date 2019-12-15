using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private SpriteRenderer render;
    private bool canWalk = true;

    public bool CanWalk { get { return canWalk && character == null; } }

    private Vector2Int pos;
    public Vector2Int Pos { set { pos = value; } get { return pos; } }

    public CharBase character;

    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void Block()
    {
        render.color = new Color(1f, 0.5f, 0.5f);
        canWalk = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public bool IsOccupant(CharBase c)
    {
        return character == c;
    }

    public bool TryOccupy(CharBase c)
    {
        if (CanWalk ||IsOccupant(c))
        {
            character = c;
            return true;
        }

        c.CheckAndResolveDeadlock(character);
        return false;
    }

    public bool TryRelease(CharBase chara)
    {
        if(chara == character)
        {
            character = null;
            return true;
        }

        return false;
    }

    public void Path()
    {
        render.color = new Color(0.5f, 0.5f, 1.0f);
    }

    public void SetSprite(Sprite sprite)
    {
        render.sprite = sprite;
        var size = TileManager.Instance.tileSize;
        transform.localScale = new Vector3(size/ render.size.x, size/render.size.y);
    }
}
