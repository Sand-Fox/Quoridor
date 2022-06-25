using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RelationsInspector.Backend.AutoBackend;

[AutoBackend]
public class NodeMono : MonoBehaviour
{
    public int score;
    public int depth;
    public Coup coup = new CoupMove(Vector2.right);

    public List<Coup> listCoup = new List<Coup>();

    [Related]
    private List<NodeMono> children = new List<NodeMono>();

    public static NodeMono Create(Coup _coup, int initialScore, int _depth, NodeMono parent)
    {
        NodeMono nodePrefab = Resources.Load<NodeMono>("NodeMono");
        NodeMono nodeMono;
        if(parent == null) nodeMono = Instantiate(nodePrefab);
        else nodeMono = Instantiate(nodePrefab, parent.transform);

        //nodeMono.coup = _coup;
        nodeMono.score = initialScore;
        nodeMono.depth = _depth;
        parent?.children.Add(nodeMono);

        nodeMono.listCoup.Add(_coup);
        nodeMono.listCoup.Add(_coup);
        nodeMono.listCoup.Add(_coup);


        nodeMono.ChangeName(15);
        return nodeMono;
    }

    public void ChangeName(int score) => name = "Score : " + score + " pour le coup " + coup;
}
