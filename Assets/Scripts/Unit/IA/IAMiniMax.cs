using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;
using System;

public class IAMiniMax : BaseIA
{

    public static string description = "MiniMax IA";

    protected override void PlayIA()
    {
        Debug.Log(GridManager.Instance.cornersDico.Count);
        Node node = new Node(null, -10000, 0);

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
        int score = pathP.Count - pathIA.Count + 2 * (nbWallIA - nbWallP);

        return score;
    }

    private Coup minMax(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore(current);
            return current.coup;
        }


        bool evenDepth = current.depth%2 == 0;

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
                
                Node node = new Node(coupWall1, (evenDepth)?10000:-10000  ,current.depth + 1);
                
                current.AddNode(node);

                minMax(node, maxDepth);

                if ((current.score < node.score && evenDepth)||(current.score > node.score && !evenDepth) || (current.depth == 0 && current.score == -10000))
                {
                    Debug.Log("current score : " + current.score + ", dept : " + current.depth);
                    Debug.Log("node score : " + node.score + ", dept : " + node.depth);
                    current.score = node.score;

                    bestCoup = node.coup;

                    /*
                    Debug.Log(current.depth);
                    Debug.Log(bestCoup);
                    */
                    
                }
                horizontalWall.OnDespawn();
            }

            //vertical
            /*
            verticalWall.transform.position = pair.Key;
            if (verticalWall.CanSpawnHere())
            {
                verticalWall.OnSpawn();
                CoupWall coupWall1 = new CoupWall(verticalWall.transform.position, Orientation.Vertical);
                Node node = new Node(coupWall1, (evenDepth) ? 10000 : -10000, current.depth + 1);
                current.AddNode(node);

                Coup c = minMax(node, maxDepth);

                if ((current.score < node.score && evenDepth) || (current.score > node.score && !evenDepth))
                {
                    current.score = node.score;
                    bestCoup = c;
                }
                verticalWall.OnDespawn();
            }
            */
        }

        BaseUnit playing = (evenDepth) ? ReferenceManager.Instance.player : ReferenceManager.Instance.enemy;
        CustomTile currentTile = playing.occupiedTile;
        /*
        foreach (CustomTile tile in currentTile.AdjacentTiles())
        {
            playing.SetUnit(tile.transform.position);
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, (evenDepth) ? 10000 : -10000, current.depth + 1);
            current.AddNode(node);

            Coup c = minMax(node, maxDepth);

            if ((current.score < node.score && evenDepth) || (current.score > node.score && !evenDepth))
            {
                current.score = node.score;
                bestCoup = c;
            }

            playing.SetUnit(currentTile.transform.position);
        }
        */


        Destroy(horizontalWall.gameObject);
        Destroy(verticalWall.gameObject);

        Debug.Log("returned score : " + current.score + ", depth : " + current.depth);
        Debug.Log("returned coup : " + bestCoup);
        return bestCoup;
    }
}