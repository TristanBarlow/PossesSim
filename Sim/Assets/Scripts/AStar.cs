using System.Collections.Generic;

public delegate bool CanWalk(int x, int y);

public class AStarPath
{
    private List<Node> Path;
    private int index = 0;
    private bool _fullPath;

    public AStarPath(bool r, List<Node> p)
    {
        _fullPath = r; Path = p;
    }

    public override string ToString() 
    {
        var str = "";
        foreach(var p in Path)
        {
            str += "(" + p.str + ") ";
        }
        return str;
    }

    public  void Reset() { index =0; }

    public bool IsEmpty => Path.Count == 0;
    public bool FullPath => _fullPath;
    public bool AtEnd => index == Path.Count - 1;
    public Node FinalNode => Path[Path.Count - 1];

    public Node Next()
    {
        if (index > Path.Count - 1) return null;
        index++;
        return Path[index];
    }
}

public class Node {
    public int x;
    public int y;
    public float h;
    public float g;

    public Node parent;
    public List<Node> children = new List<Node>();
    public float f { get { return g + h; } }
    public string str  => "x:" + x + "y:" + y; 
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
    public static AStarPath GetPath(int x1, int y1, int x2, int y2, CanWalk canWalk, bool allowIncomplete = true)
    {
        var open = new Dictionary<string, Node>();
        var closed = new Dictionary<string, Node>();
        var strt = new Node(x1, y1, null, GetH(x1, y1, x2, y2), 0);

        var closest = strt;

        open.Add(strt.str, strt);
        while(open.Count > 0)
        {
            var current = LowestF(open);
            open.Remove(current.str);

            closed.Add(current.str, current);

            if(current.x == x2 && current.y == y2)
            {
                return MakePath(current, true);
            }

            if (current.h < closest.h)
            {
                closest = current;
            }

            current.AddIfCanWalk(canWalk, 0, -1);
            current.AddIfCanWalk(canWalk, -1, 0);

            current.AddIfCanWalk(canWalk, 0, 1);
            current.AddIfCanWalk(canWalk, 1, 0);

            /*current.AddIfCanWalk(canWalk, -1, -1);*/
            /*current.AddIfCanWalk(canWalk, 1, 1);*/
            /*current.AddIfCanWalk(canWalk, 1, -1);*/
            /*current.AddIfCanWalk(canWalk, -1, 1);*/

            foreach(var child in current.children)
            {
                if (closed.ContainsKey(child.str))
                    continue;

                child.g = current.g + 1;
                child.h = GetH(child.x, child.y, x2, y2);

                if (open.ContainsKey(child.str))
                    continue;

                open.Add(child.str, child);
            }
        }

        if(allowIncomplete)
            return MakePath(closest, false);

        return null;
    }

    private static AStarPath MakePath(Node current, bool complete )
    {
        var path = new List<Node>();
        var last = current;
        while (current != null)
        {
            path.Add(current);
            current = current.parent;
        }
        path.Reverse();
        return new AStarPath(complete, path);
    }

    static float GetH(int x1, int y1, int x2, int y2)
    {
        return ((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2));
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


