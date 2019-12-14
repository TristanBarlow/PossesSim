using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBase : MonoBehaviour
{

    Stack<Node> path;
    Tile targetTile;
    public float speed = 1.0f;
    public bool wait = false;
    // Update is called once per frame
    void Update()
    {
        TryMove();
    }

    private void TryMove()
    {
        if (wait) return;

        if (targetTile != null)
        {
            float step = speed * Time.deltaTime;
            var target = targetTile.transform;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                targetTile = null;
            }
        }
        else if (path != null && path.Count > 0)
        {
            var node = path.Pop();
            targetTile = TileManager.Instance.GetTile(node.x, node.y);
        }
        else
        {
            var point = TileManager.Instance.RandomPoint();
            MoveTo(point.x, point.y);
        }
    }

    public void MoveTo(int x, int y )
    {
        path = TileManager.Instance.GetPath(transform.position, x, y);
    }
}
