using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

public class IAMiniMax : BaseIA
{
    public static string description = "IA qui choisit le meilleur coup à jouer en utilisant un algorithme Mini Max";

    protected override void PlayIA()
    {
        Node node = new Node(null, 0);
        Coup coup = Max(node, 2);

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
    }

    private int CalculScore()
    {
        List<CustomTile> pathIA = PathFinding.Instance.GetWiningPath(this);
        List<CustomTile> pathP = PathFinding.Instance.GetWiningPath(ReferenceManager.Instance.player);
        int nbWallIA = wallCount;
        int nbWallP = UIManager.Instance.wallCount;

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

        Coup bestCoup = null;
        HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);

        
        foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
        {
            horizontalWall.transform.position = pair.Key;
            if (horizontalWall.CanSpawnHere())
            {
                horizontalWall.OnSpawn();
                CoupWall coupWall = new CoupWall(horizontalWall.transform.position, Orientation.Horizontal);
                Node node = new Node(coupWall, current.depth + 1);

                current.AddChild(node);

                Min(node, maxDepth);

                if (current.score < node.score || current.score == Node.initialScore)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }
                horizontalWall.OnDespawn();
            }

            //Vertical Wall
        }
        

        CustomTile IATile = occupiedTile;

        foreach (CustomTile tile in IATile.AdjacentTiles())
        {
            SetUnitNoAnimation(tile.transform.position);
            
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);
            Min(node, maxDepth);

            if (current.score < node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
        }

        SetUnitNoAnimation(IATile.transform.position);

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

        Coup bestCoup = null;
        HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
        VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);

        
        foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
        {
            horizontalWall.transform.position = pair.Key;
            if (horizontalWall.CanSpawnHere())
            {
                horizontalWall.OnSpawn();
                CoupWall coupWall = new CoupWall(horizontalWall.transform.position, Orientation.Horizontal);
                Node node = new Node(coupWall, current.depth + 1);
                current.AddChild(node);

                Max(node, maxDepth);

                if (node.score < current.score || current.score == Node.initialScore)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }
                horizontalWall.OnDespawn();
            }

            //Vertical Wall
        }
        

        BaseUnit player = ReferenceManager.Instance.player;
        CustomTile playerTile = player.occupiedTile;

        foreach (CustomTile tile in playerTile.AdjacentTiles())
        {
            player.SetUnitNoAnimation(tile.transform.position);

            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);
            Max(node, maxDepth);

            if (current.score > node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
        }

        player.SetUnitNoAnimation(playerTile.transform.position);

        Destroy(horizontalWall.gameObject);
        Destroy(verticalWall.gameObject);
        return bestCoup;
    }
}