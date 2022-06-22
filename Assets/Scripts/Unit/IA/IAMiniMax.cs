using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

public class IAMiniMax : BaseIA
{

    protected override void PlayIA()
    {
        Node node = new Node(null, 0);

        Coup coup = minMax(node, 2);
        Debug.Log(coup);

        if(coup is CoupWall coupWall)
        {

            GameObject wallObject = PhotonNetwork.Instantiate("Wall/" + coupWall.orientation + "Wall", Vector3.zero, Quaternion.identity);

            Vector3 vec = new Vector3(coupWall.coord[0], coupWall.coord[1], 0);
            wallObject.GetComponent<CustomWall>().view.RPC("SetWall", RpcTarget.All, vec);
            wallCount--;
        }
    }


    private int CalculScore(Node node)
    {
        // recuperer la distance de la fin de l'IA et celle du joueur
        List<CustomTile> pathIA = GetBestPath();
        List<CustomTile> pathP = GetPlayerBestPath();

        // recuperer le nombre de mur restant a l'IA et celle du joueur
        int nbWallIA = this.wallCount;
        int nbWallP = UIManager.Instance.wallCount;
        // fonction du type f(nbMurIA, distIA, nbMurJ, distJ) = nbMurIA * 2 +- distIA - nbMurJ * 2 + distJ       -> a voir si pertinent 
        int score = -(pathP.Count - pathIA.Count + 2 * (nbWallIA - nbWallP));

        return score;
    }

    private Coup minMax(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore(current);
            return current.coup;
        }

        HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);
        Coup bestCoup = null;
        foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
        {
            //horizontal
            horizontalWall.transform.position = pair.Key;
            if (horizontalWall.CanSpawnHere())
            {
                horizontalWall.OnSpawn();
                CoupWall coupWall1 = new CoupWall(horizontalWall.transform.position, Orientation.Horizontal);
                Node node = new Node(coupWall1, current.depth + 1);
                current.AddNode(node);

                Coup c = minMax(node, maxDepth);

                if (current.score < node.score)
                {
                    current.score = node.score;
                    bestCoup = c;
                }
                horizontalWall.OnDespawn();
            }

            verticalWall.transform.position = pair.Key;
            if (verticalWall.CanSpawnHere())
            {
                verticalWall.OnSpawn();
                CoupWall coupWall1 = new CoupWall(verticalWall.transform.position, Orientation.Vertical);
                Node node = new Node(coupWall1, current.depth + 1);
                current.AddNode(node);

                Coup c = minMax(node, maxDepth);

                if (current.score < node.score)
                {
                    current.score = node.score;
                    bestCoup = c;
                }

                verticalWall.OnDespawn();
            }
        }
        Destroy(horizontalWall.gameObject);
        Destroy(verticalWall.gameObject);
        return bestCoup;
    }
}