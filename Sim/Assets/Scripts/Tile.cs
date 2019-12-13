using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private SpriteRenderer render;
    public bool canWalk = true;

    private Vector2 pos;

    public Vector2 Pos { set { pos = value; } get { return pos; } }

    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Block()
    {
        render.color = new Color(1f, 0.5f, 0.5f);
        canWalk = false;
    }

    public void Path()
    {
        render.color = new Color(0.5f, 0.5f, 1.0f);
    }

    public void SetSprite(Sprite sprite)
    {
        render.sprite = sprite;
    }
}
