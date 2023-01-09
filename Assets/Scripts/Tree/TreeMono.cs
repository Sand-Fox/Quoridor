using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RelationsInspector.Backend.AutoBackend;

[AutoBackend]
public class TreeMono : MonoBehaviour
{
    private void Start()
    {
        NodeMono nodeMono0 = NodeMono.Create(new CoupMove(Vector2.right), 25, 0, null);

        NodeMono nodeMono1 = NodeMono.Create(new CoupWall(Vector2.left, Orientation.Horizontal), 10, 1, nodeMono0);
        NodeMono nodeMono2 = NodeMono.Create(new CoupMove(Vector2.up), 12, 1, nodeMono0);

        NodeMono nodeMono3 = NodeMono.Create(new CoupMove(Vector2.down), 14, 2, nodeMono1);
        NodeMono nodeMono4 = NodeMono.Create(new CoupWall(Vector2.left, Orientation.Vertical), 10, 2, nodeMono1);
        NodeMono nodeMono5 = NodeMono.Create(new CoupMove(Vector2.up), 5, 2, nodeMono2);
    }

}
