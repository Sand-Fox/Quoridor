using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

public class IAMiniMax : BaseIA
{
    public static string description = "Mini Max IA";

    protected override void PlayIA()
    {
        Node node = new Node(null, -10000, 0);

        Coup coup = Max(node, 2);
        Debug.Log(coup);

        if (coup is CoupWall coupWall)
        {
            GameObject wallObject = PhotonNetwork.Instantiate("Wall/" + coupWall.orientation + "Wall", Vector3.zero, Quaternion.identity);
            Vector3 position = new Vector3(coupWall.coord[0], coupWall.coord[1], 0);
            wallObject.GetComponent<CustomWall>().view.RPC("SetWall", RpcTarget.All, position);
            wallCount--;
        }

        if(coup is CoupMove coupMove)
        {
            Vector3 position = new Vector3(coupMove.coord[0], coupMove.coord[1], 0);
            SetUnit(position);
        }
        Debug.Log("La fonction est fini");
    }

    private int CalculScore()
    {
        List<CustomTile> pathIA = GetBestPath();
        List<CustomTile> pathP = GetPlayerBestPath();
        int nbWallIA = wallCount;
        int nbWallP = UIManager.Instance.wallCount;
        //int score = pathP.Count - pathIA.Count + 2 * (nbWallIA - nbWallP);
        int score = -pathIA.Count;
        return score;
    }

    private Coup Max(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);
        Coup bestCoup = null;
        
        foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
        {
            horizontalWall.transform.position = pair.Key;
            if (horizontalWall.CanSpawnHere())
            {
                horizontalWall.OnSpawn();
                CoupWall coupWall = new CoupWall(horizontalWall.transform.position, Orientation.Horizontal);
                Node node = new Node(coupWall, 10000, current.depth + 1);
                current.AddNode(node);

                Min(node, maxDepth);

                if (current.score < node.score)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }
                horizontalWall.OnDespawn();
            }

            
            verticalWall.transform.position = pair.Key;
            if (verticalWall.CanSpawnHere())
            {
                verticalWall.OnSpawn();
                CoupWall coupWall1 = new CoupWall(verticalWall.transform.position, Orientation.Vertical);
                Node node = new Node(coupWall1, 10000, current.depth + 1);
                current.AddNode(node);

                Min(node, maxDepth);

                if (current.score < node.score)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }

                verticalWall.OnDespawn();
            }
        }

        BaseUnit IA = ReferenceManager.Instance.enemy;
        CustomTile currentTile = IA.occupiedTile;
        
        foreach (CustomTile tile in currentTile.AdjacentTiles())
        {
            Vector3 posVec3 = tile.transform.position;
            Vector2 posVec2 = new Vector2(posVec3[0], posVec3[1]);
            IA.SetUnit(posVec3);
            CoupMove move = new CoupMove(posVec2);

            Node node = new Node(move, 10000, current.depth + 1);
            current.AddNode(node);

            Min(node, maxDepth);

            if (current.score < node.score)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
            IA.SetUnit(currentTile.transform.position);
        }
        

        Destroy(horizontalWall.gameObject);
        Destroy(verticalWall.gameObject);
        return bestCoup;

    }

    private Coup Min(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);
        Coup bestCoup = null;
        
        foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
        {
            horizontalWall.transform.position = pair.Key;
            if (horizontalWall.CanSpawnHere())
            {
                horizontalWall.OnSpawn();
                CoupWall coupWall = new CoupWall(horizontalWall.transform.position, Orientation.Horizontal);
                Node node = new Node(coupWall, -10000, current.depth + 1);
                current.AddNode(node);

                Max(node, maxDepth);

                if (current.score > node.score)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }
                horizontalWall.OnDespawn();
            }
            
            verticalWall.transform.position = pair.Key;
            if (verticalWall.CanSpawnHere())
            {
                verticalWall.OnSpawn();
                CoupWall coupWall1 = new CoupWall(verticalWall.transform.position, Orientation.Vertical);
                Node node = new Node(coupWall1, -10000, current.depth + 1);
                current.AddNode(node);

                Max(node, maxDepth);

                if (current.score > node.score)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }

                verticalWall.OnDespawn();
            }
            
        }
        
        
        BaseUnit player = ReferenceManager.Instance.player;
        CustomTile currentTile = player.occupiedTile;
        foreach (CustomTile tile in currentTile.AdjacentTiles())
        {

            Vector3 posVec3 = tile.transform.position;
            Vector2 posVec2 = new Vector2(posVec3[0], posVec3[1]);
            player.SetUnit(posVec3);
            CoupMove move = new CoupMove(posVec2);
            Node node = new Node(move, -10000, current.depth + 1);
            current.AddNode(node);

            Max(node, maxDepth);

            if (current.score > node.score)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
            player.SetUnit(currentTile.transform.position);
        }
        

        Destroy(horizontalWall.gameObject);
        Destroy(verticalWall.gameObject);
        return bestCoup;
    }


}