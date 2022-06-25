using System.Collections.Generic;

public class Node
{
    public int score;
    public int depth;
    public Coup coup;
    private List<Node> nextNodes = new List<Node>();

    public Node(Coup _coup, int initialScore, int _depth)
    {
        coup = _coup;
        score = initialScore;
        depth = _depth;
    }

    public void AddNode(Node n) => nextNodes.Add(n);
}