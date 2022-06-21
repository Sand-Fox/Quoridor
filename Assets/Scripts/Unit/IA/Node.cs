using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Dictionary<Vector2, CustomTile> plateau;
    private int score;
    private int depth;
    private List<Node> nextNodes = new List<Node>();

    private Node(Dictionary<Vector2, CustomTile> plateau, int depth)
    {
        this.plateau = plateau;
        this.depth = depth;
    }


    public void AddNode(Node n) => this.nextNodes.Add(n);
    public void setScore(int score) => this.score = score;

    public int generateNextCoup()
    {
        return 1;
    }

}
