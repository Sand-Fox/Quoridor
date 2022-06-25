using System.Collections.Generic;
using RelationsInspector.Backend.AutoBackend;
using UnityEngine;

[AutoBackend]
public class Node
{
<<<<<<< Updated upstream
=======
    public static readonly int initialScore = 10000;

>>>>>>> Stashed changes
    public int score;
    public int depth;
    public Coup coup;
    private List<Node> children = new List<Node>();

    [Related]
    public Node parent;

<<<<<<< Updated upstream
    public Node(Coup _coup, int initialScore, int _depth)
    {
        coup = _coup;
        score = initialScore;
        depth = _depth;
    }

    public void AddNode(Node n) => nextNodes.Add(n);
}
=======
    public Node(Coup _coup, int _depth)
    {
        coup = _coup;
        depth = _depth;
        score = initialScore;
    }

    public void AddChild(Node n) => children.Add(n);
}
>>>>>>> Stashed changes
