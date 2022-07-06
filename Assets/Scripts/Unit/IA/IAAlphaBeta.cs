using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

public class IAAlphaBeta : BaseIA
{
    public static string description = "IA qui choisit le meilleur coup à jouer en utilisant l'algorithme Mini Max";

    protected override void PlayIA()
    {
        if (wallCount == 0)
        {
            List<CustomTile> path = PathFinding.Instance.GetWiningPath(this);
            if (path.Count != 0) SetUnit(path[0].transform.position);
            return;
        }

        Node node = new Node(null, 0);
        Coup coup = Max(node, 2, -10000, 10000, false);

        if (coup is CoupWall coupWall)
        {
            Debug.Log("Coup is wall :" + "(" + coup.coord[0]+ ","+ coup.coord[1] + ")");
            Vector3 wallPosition = new Vector3(coupWall.coord[0], coupWall.coord[1], 0);
            Orientation orientation = coupWall.orientation;
            SpawnWall(wallPosition, orientation);
        }

        if(coup is CoupMove coupMove)
        {
            Debug.Log("Coup is Move :" + coup.coord[0]+ ","+ coup.coord[1] + ")");
            Vector3 position = new Vector3(coupMove.coord[0], coupMove.coord[1], 0);
            SetUnit(position);
        }
    }

    private int CalculScore()
    {
        List<CustomTile> pathIA = PathFinding.Instance.GetWiningPath(this);
        List<CustomTile> pathP = PathFinding.Instance.GetWiningPath(OtherUnit());
        int nbWallIA = wallCount;
        int nbWallP = OtherUnit().wallCount;

        int distMax = GridManager.MAXPATH;
        int distP = pathP.Count;
        int distIA = pathIA.Count;
        int score = -distIA;
        return score;
    }

    private Coup Max(Node current, int maxDepth, int alpha, int beta, bool doesWall=true)
    {
        if(current.depth!=0)
        Debug.Log("Max : profondeur : " + current.depth + "type : " + ((current.coup is CoupMove)?"Move ":"Wall" )+  "; coup : (" + current.coup.coord[0]+ ","+ current.coup.coord[1] + ")");
        
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }
        Coup bestCoup = null;

        CustomTile IATile = occupiedTile;

        foreach (CustomTile tile in IATile.AdjacentTiles())
        {
            SetUnitWhenTesting(tile.transform.position);
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);

            Min(node, maxDepth, alpha, beta, doesWall);

            if (current.score < node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }

            alpha = Mathf.Max(alpha, node.score);

            if (beta <= alpha)
            {
                this.SetUnitWhenTesting(IATile.transform.position);
                return bestCoup;
            }
            
        }
        SetUnitWhenTesting(IATile.transform.position);

        if (doesWall && wallCount > 0)
        {
            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Min(node, maxDepth, alpha, beta, doesWall);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    alpha = Mathf.Max(alpha, node.score);

                    if (beta <= alpha)
                    {
                        Debug.Log("Max: return wall Horizontal = " + "(" + bestCoup.coord[0]+ ","+ bestCoup.coord[1] + ")");
                        return bestCoup;
                    }
                }
            
                if (VerticalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    Min(node, maxDepth, alpha, beta, doesWall);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    alpha = Mathf.Max(alpha, node.score);

                    if (beta <= alpha)
                    {
                        Debug.Log("Max: return wall Vertical = " + "(" + bestCoup.coord[0]+ ","+ bestCoup.coord[1] + ")");
                        return bestCoup;
                    }
                }
            }
        }
        Debug.Log("Max: return best coup = " + "(" + bestCoup.coord[0]+ ","+ bestCoup.coord[1] + ")");
        return bestCoup;
    }

    private Coup Min(Node current, int maxDepth, int alpha, int beta, bool doesWall)
    {
        if (current.depth !=0)
        Debug.Log("Min : profondeur : " + current.depth + "type : " + ((current.coup is CoupMove)?"Move ":"Wall" )+  "; coup : (" + current.coup.coord[0]+ ","+ current.coup.coord[1] + ")");

        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        BaseUnit player = OtherUnit();
        Coup bestCoup = null;
        CustomTile playerTile = player.occupiedTile;

        foreach (CustomTile tile in playerTile.AdjacentTiles())
        {
            player.SetUnitWhenTesting(tile.transform.position);
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);

            Max(node, maxDepth, alpha, beta, doesWall);

            if (current.score > node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }

            beta = Mathf.Min(beta, node.score);
        
            if (beta <= alpha)
            {
                player.SetUnitWhenTesting(playerTile.transform.position);
                Debug.Log("Min: return move = " + "(" + bestCoup.coord[0]+ ","+ bestCoup.coord[1] + ")");
                return bestCoup;
            }
        }
            player.SetUnitWhenTesting(playerTile.transform.position);


        if (doesWall && player.wallCount > 0)
        {
            foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    player.SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth, alpha, beta, doesWall);

                    if (node.score < current.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    player.DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);

                    beta = Mathf.Min(beta, node.score);
                    if (beta <= alpha)
                    {
                        return bestCoup;
                    }
                }
            
                if (VerticalWall.CanSpawnHere(pair.Value))
                {
                    player.SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth, alpha, beta, doesWall);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    player.DespawnWallWhenTesting(pair.Key, Orientation.Vertical);

                    beta = Mathf.Min(beta, node.score);
                    if (beta <= alpha)
                    {
                        return bestCoup;
                    }
                }
            }
        }
        return bestCoup;
    }
}