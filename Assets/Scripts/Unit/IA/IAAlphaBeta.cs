using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

public class IAAlphaBeta : BaseIA
{
    public static string description = "IA qui choisit le meilleur coup à jouer en utilisant l'algorithme Alpha Beta";

    protected override void PlayIA()
    {
        if (wallCount == 0)
        {
            List<CustomTile> path = PathFinding.Instance.GetWiningPath(this);
            if (path.Count != 0) SetUnit(path[0].transform.position);
            return;
        }

        Node node = new Node(null, 0);
        Coup coup = Max(node, 2, -10000, 10000);

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
        int nbWallP = ReferenceManager.Instance.player.wallCount;

        int distMax = GridManager.MAXPATH;
        int distP = pathP.Count;
        int distIA = pathIA.Count;
        int score = distP*distP - distIA*distIA + nbWallIA - nbWallP;
        return score;
    }

    private Coup Max(Node current, int maxDepth, int alpha, int beta)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }
        Coup bestCoup = null;

        /*since moving is mostlikely going to be a better move we shall evaluate them first*/

        CustomTile IATile = occupiedTile;
        foreach (CustomTile tile in IATile.AdjacentTiles())
        {
            SetUnitNoAnimation(tile.transform.position);
            
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);
            Min(node, maxDepth, alpha, beta);

            if (current.score < node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }

            alpha = (alpha<node.score)?node.score:alpha;
            if(beta<=alpha)
            {
                SetUnitNoAnimation(IATile.transform.position);
                return bestCoup;
            } 
        }

        SetUnitNoAnimation(IATile.transform.position);



        if(wallCount >0){
            
            HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
            VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);

            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                horizontalWall.transform.position = pair.Key;
                if (horizontalWall.CanSpawnHere())
                {
                    wallCount--;
                    horizontalWall.OnSpawn();
                    CoupWall coupWall = new CoupWall(horizontalWall.transform.position, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    current.AddChild(node);

                    Min(node, maxDepth, alpha, beta);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    horizontalWall.OnDespawn();
                    wallCount++;

                    alpha = (alpha<node.score)?node.score:alpha;
                    if(beta<=alpha)
                    {
                        Destroy(horizontalWall.gameObject);
                        Destroy(verticalWall.gameObject);
                        return bestCoup;
                    }
                }
            }


            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                verticalWall.transform.position = pair.Key;
                if (verticalWall.CanSpawnHere())
                {
                    wallCount--;
                    verticalWall.OnSpawn();
                    CoupWall coupWall = new CoupWall(verticalWall.transform.position, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    current.AddChild(node);

                    Min(node, maxDepth, alpha, beta);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    verticalWall.OnDespawn();
                    wallCount++;

                    alpha = (alpha<node.score)?node.score:alpha;
                    if(beta<=alpha)
                    {
                        Destroy(horizontalWall.gameObject);
                        Destroy(verticalWall.gameObject);
                        return bestCoup;
                    }
                }
            }
            Destroy(horizontalWall.gameObject);
            Destroy(verticalWall.gameObject);
        }
        
        return bestCoup;
    }




    private Coup Min(Node current, int maxDepth, int alpha, int beta)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        BaseUnit player = ReferenceManager.Instance.player;
        Coup bestCoup = null;

        /*since moving is mostlikely going to be a better move we shall evaluate them first*/
        CustomTile playerTile = player.occupiedTile;

        foreach (CustomTile tile in playerTile.AdjacentTiles())
        {
            player.SetUnitNoAnimation(tile.transform.position);

            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);
            Max(node, maxDepth, alpha, beta);

            if (current.score > node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
            beta = (beta>node.score)?node.score:beta;
            if(beta<=alpha)
            {
                SetUnitNoAnimation(playerTile.transform.position);
                return bestCoup;
            }
        }

        if(player.wallCount>0){
            HorizontalWall horizontalWall = Instantiate(ReferenceManager.Instance.horizontalWallPrefab);
            VerticalWall verticalWall = Instantiate(ReferenceManager.Instance.verticalWallPrefab);

            foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
            {
                horizontalWall.transform.position = pair.Key;
                if (horizontalWall.CanSpawnHere())
                {
                    player.wallCount--;
                    horizontalWall.OnSpawn();
                    CoupWall coupWall = new CoupWall(horizontalWall.transform.position, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);
                    current.AddChild(node);

                    Max(node, maxDepth, alpha, beta);

                    if (node.score < current.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    horizontalWall.OnDespawn();
                    player.wallCount++;

                    beta = (beta>node.score)?node.score:beta;
                    if(beta<=alpha)
                    {
                        Destroy(horizontalWall.gameObject);
                        Destroy(verticalWall.gameObject);
                        return bestCoup;
                    }
                }             
            }

            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                verticalWall.transform.position = pair.Key;
                if (verticalWall.CanSpawnHere())
                {
                    wallCount--;
                    verticalWall.OnSpawn();
                    CoupWall coupWall = new CoupWall(verticalWall.transform.position, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    current.AddChild(node);

                    Max(node, maxDepth, alpha, beta);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    verticalWall.OnDespawn();
                    wallCount++;

                    beta = (beta>node.score)?node.score:beta;
                    if(beta<=alpha)
                    {
                        Destroy(horizontalWall.gameObject);
                        Destroy(verticalWall.gameObject);
                        return bestCoup;
                    }
                }
                
            }
            Destroy(horizontalWall.gameObject);
            Destroy(verticalWall.gameObject);
        }

        

        player.SetUnitNoAnimation(playerTile.transform.position);

        
        return bestCoup;
    }
}