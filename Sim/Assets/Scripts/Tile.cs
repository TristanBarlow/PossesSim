using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private SpriteRenderer render;
    public bool canWalk = true;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("i");
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

    public void SetSprite(Sprite sprite)
    {
        render.sprite = sprite;
    }
}
