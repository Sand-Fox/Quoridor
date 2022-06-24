using System.Collections.Generic;

public class Node
{
    public int score;
    public int depth;
    public Coup coup;
    private List<Node> nextNodes = new List<Node>();

    public Node(Coup coup, int score, int depth)
    {
        this.coup = coup;
        this.depth = depth;
    }


    public void AddNode(Node n) => this.nextNodes.Add(n);
}
