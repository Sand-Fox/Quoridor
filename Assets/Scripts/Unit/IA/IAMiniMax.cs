using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Debug = UnityEngine.Debug;

public class IAMiniMax : BaseIA
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
        Coup coup = Max(node, 2);

        if (coup is CoupWall coupWall)
        {
            Vector3 wallPosition = new Vector3(coupWall.coord[0], coupWall.coord[1], 0);
            Orientation orientation = coupWall.orientation;
            SpawnWall(wallPosition, orientation);
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

    private Coup Max(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        Coup bestCoup = null;

        if (wallCount > 0)
        {
            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Min(node, maxDepth);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                }
            }

            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                if (VerticalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    Min(node, maxDepth);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Vertical);
                }
            }
        }

        CustomTile IATile = occupiedTile;

        foreach (CustomTile tile in IATile.AdjacentTiles())
        {
            SetUnitWhenTesting(tile.transform.position);
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);

            Min(node, maxDepth);

            if (current.score < node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
        }

        SetUnitWhenTesting(IATile.transform.position);
        return bestCoup;
    }

    private Coup Min(Node current, int maxDepth)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        BaseUnit player = ReferenceManager.Instance.player;
        Coup bestCoup = null;

        if (player.wallCount > 0)
        {
            foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth);

                    if (node.score < current.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                }
            }
        
            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                if (VerticalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    DespawnWallWhenTesting(pair.Key, Orientation.Vertical);
                }
            }
        }

        CustomTile playerTile = player.occupiedTile;

        foreach (CustomTile tile in playerTile.AdjacentTiles())
        {
            player.SetUnitWhenTesting(tile.transform.position);
            CoupMove move = new CoupMove(tile.transform.position);
            Node node = new Node(move, current.depth + 1);

            Max(node, maxDepth);

            if (current.score > node.score || current.score == Node.initialScore)
            {
                current.score = node.score;
                bestCoup = node.coup;
            }
        }

        player.SetUnitWhenTesting(playerTile.transform.position);
        return bestCoup;
    }
}