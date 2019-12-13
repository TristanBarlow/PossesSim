using System.Collections.Generic;

public delegate bool CanWalk(int x, int y);

class Node {
    public int x;
    public int y;
    public float h;
    public float g;

    public Node parent;
    public List<Node> children = new List<Node>();
    public float f { get { return g + h; } }
    public string str { get { return "x" + x + "y" + y; } }
    public Node(int x, int y, Node parent, float h, float g)
    {
        this.x = x; this.y = y; this.h = h; this.g = g; this.parent = parent;
    }
    public void AddIfCanWalk(CanWalk canWalk, int dx, int dy)
    {
        if (canWalk(x + dx, y + dy))
            children.Add(new Node(x + dx, y + dy, this, 0, 0));
    }
}

static class AStar
{
    public static List<Node> GetPath(int x1, int y1, int x2, int y2, CanWalk canWalk)
    {
        var open = new Dictionary<string, Node>();
        var closed = new Dictionary<string, Node>();
        var strt = new Node(x1, y1, null, 0, 0);

        open.Add(strt.str, strt);
        while(open.Count > 0)
        {
            var current = LowestF(open);
            open.Remove(current.str);

            closed.Add(current.str, current);

            if(current.x == x2 && current.y == y2)
            {
                var path = new List<Node>();
                while(current != null)
                {
                    path.Insert(0, current);
                    current = current.parent;
                }
                    return path;
            }

            current.AddIfCanWalk(canWalk, -1, -1);
            current.AddIfCanWalk(canWalk, 0, -1);
            current.AddIfCanWalk(canWalk, -1, 0);

            current.AddIfCanWalk(canWalk, 1, 1);
            current.AddIfCanWalk(canWalk, 0, 1);
            current.AddIfCanWalk(canWalk, 1, 0);

            current.AddIfCanWalk(canWalk, 1, -1);
            current.AddIfCanWalk(canWalk, -1, 1);

            foreach(var child in current.children)
            {
                if (closed.ContainsKey(child.str))
                    continue;

                child.g = current.g + 1;
                child.h = ((child.x - x2) * (child.x - x2)) + ((child.y - y2) * (child.y - y2));

                if (open.ContainsKey(child.str))
                    continue;

                open.Add(child.str, child);
            }
        }
        return null;
    }


    static Node LowestF(Dictionary<string, Node> nodes)
    {
        var lowest = 10000.0;
        Node lNode = null;
        foreach(var node in nodes)
        {
            if (node.Value.f < lowest)
            {
                lowest = node.Value.f;
                lNode = node.Value;
            }
        }
        return lNode;
    }

}


