using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class IAAlphaBeta : BaseIA
{
    public static string description = "IA qui choisit le meilleur coup à jouer en utilisant l'algorithme Alpha Beta";
    public float[] weight = new float[4];

    protected override void PlayIA()
    {
        if (wallCount == 0)
        {
            List<CustomTile> path = PathFinding.Instance.GetWiningPath(this);
            if (path.Count != 0) SetUnit(path[0].transform.position);
            return;
        }

        Node node = new Node(null, 0);
        Coup coup = Max(node, 2, -100000000, 100000000, true, false);

        if (coup is CoupWall coupWall)
        {
            Debug.Log("Coup is wall : (" + coup.coord[0]+ ","+ coup.coord[1] + ")");
            Vector3 wallPosition = new Vector3(coupWall.coord[0], coupWall.coord[1], 0);
            Orientation orientation = coupWall.orientation;
            SpawnWall(wallPosition, orientation);
        }

        if(coup is CoupMove coupMove)
        {
            Debug.Log("Coup is Move : (" + coup.coord[0]+ ","+ coup.coord[1] + ")");
            Vector3 position = new Vector3(coupMove.coord[0], coupMove.coord[1], 0);
            SetUnit(position);
        }
    }

    private float CalculScore()
    {
        List<CustomTile> pathIA = PathFinding.Instance.GetWiningPath(this);
        List<CustomTile> pathP = PathFinding.Instance.GetWiningPath(OtherUnit());
        int nbWallIA = wallCount;
        int nbWallP = OtherUnit().wallCount;

        int distP = pathP.Count;
        int distIA = pathIA.Count;
        float score = weight[0]*distP + weight[1]*distIA + weight[2]* nbWallP + weight[3]*nbWallIA;

        
        return score;
    }

    private Coup Max(Node current, int maxDepth, float alpha, float beta, bool doesWall=true, bool doesMove=true)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        Coup bestCoup = null;
        CustomTile IATile = occupiedTile;

        if (doesMove)
        {
            foreach (CustomTile tile in IATile.AdjacentTiles())
            {
                SetUnitWhenTesting(tile.transform.position);
                CoupMove move = new CoupMove(tile.transform.position);
                Node node = new Node(move, current.depth + 1);

                Min(node, maxDepth, alpha, beta, doesWall, doesMove);
                Debug.Log("Min : profondeur : " + node.depth + ", type : " + ((node.coup is CoupMove) ? "Move " : "Wall") + "; coup : (" + node.coup.coord[0] + "," + node.coup.coord[1] + ")" + "\n SCORE = " + node.score);

                if (current.score < node.score || current.score == Node.initialScore)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }

                alpha = Mathf.Max(alpha, node.score);

                if (beta <= alpha)
                {
                    SetUnitWhenTesting(IATile.transform.position);
                    return bestCoup;
                }

            }
            SetUnitWhenTesting(IATile.transform.position);
        }
        
        if (doesWall && wallCount > 0)
        {
            foreach(KeyValuePair < Vector2, CustomCorner > pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Min(node, maxDepth, alpha, beta, doesWall, doesMove);
                    Debug.Log("Min : profondeur : " + node.depth + ", type : " + ((node.coup is CoupMove)?"Move ":"Wall" )+  "; coup : (" + node.coup.coord[0]+ ","+ node.coup.coord[1] + ")" + "\n SCORE = "+ node.score);

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

                    Min(node, maxDepth, alpha, beta, doesWall, doesMove);
                    Debug.Log("Min : profondeur : " + node.depth + ", type : " + ((node.coup is CoupMove)?"Move ":"Wall" )+  "; coup : (" + node.coup.coord[0]+ ","+ node.coup.coord[1] + ")" + "\n SCORE = "+ node.score);

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
        Debug.Log("Max: return wall Vertical = " + "(" + bestCoup.coord[0]+ ","+ bestCoup.coord[1] + ")");
        return bestCoup;
    }

    private Coup Min(Node current, int maxDepth, float alpha, float beta, bool doesWall=true, bool doesMove = true)
    {
        if (current.depth == maxDepth)
        {
            current.score = CalculScore();
            return current.coup;
        }

        Coup bestCoup = null;
        BaseUnit player = OtherUnit();
        CustomTile playerTile = player.occupiedTile;

        if (doesMove)
        {
            foreach (CustomTile tile in playerTile.AdjacentTiles())
            {
                player.SetUnitWhenTesting(tile.transform.position);
                CoupMove move = new CoupMove(tile.transform.position);
                Node node = new Node(move, current.depth + 1);

                Max(node, maxDepth, alpha, beta, doesWall, doesMove);
                Debug.Log("Max : profondeur : " + node.depth + ", type : " + ((node.coup is CoupMove) ? "Move " : "Wall") + "; coup : (" + node.coup.coord[0] + "," + node.coup.coord[1] + ")" + "\n SCORE = " + node.score);

                if (current.score > node.score || current.score == Node.initialScore)
                {
                    current.score = node.score;
                    bestCoup = node.coup;
                }

                beta = Mathf.Min(beta, node.score);

                if (beta <= alpha)
                {
                    player.SetUnitWhenTesting(playerTile.transform.position);
                    Debug.Log("Min: return move = " + "(" + bestCoup.coord[0] + "," + bestCoup.coord[1] + ")");
                    return bestCoup;
                }
            }
            player.SetUnitWhenTesting(playerTile.transform.position);
        }
        
        if (doesWall && player.wallCount > 0)
        {
            foreach (KeyValuePair<Vector2, CustomCorner> pair in GridManager.Instance.cornersDico)
            {
                if (HorizontalWall.CanSpawnHere(pair.Value))
                {
                    player.SpawnWallWhenTesting(pair.Key, Orientation.Horizontal);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Horizontal);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth, alpha, beta, doesWall, doesMove);
                    Debug.Log("Max : profondeur : " + node.depth + ", type : " + ((node.coup is CoupMove)?"Move ":"Wall" )+  "; coup : (" + node.coup.coord[0]+ ","+ node.coup.coord[1] + ")" + "\n SCORE = "+ node.score);

                    if (node.score < current.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    player.DespawnWallWhenTesting(pair.Key, Orientation.Horizontal);

                    beta = Mathf.Min(beta, node.score);
                    if (beta <= alpha)
                    {
                        Debug.Log("Min: return move = " + "(" + bestCoup.coord[0]+ ","+ bestCoup.coord[1] + ")");
                        return bestCoup;
                    }
                }
            
                if (VerticalWall.CanSpawnHere(pair.Value))
                {
                    player.SpawnWallWhenTesting(pair.Key, Orientation.Vertical);
                    CoupWall coupWall = new CoupWall(pair.Key, Orientation.Vertical);
                    Node node = new Node(coupWall, current.depth + 1);

                    Max(node, maxDepth, alpha, beta, doesWall, doesMove);
                    Debug.Log("Max : profondeur : " + node.depth + ", type : " + ((node.coup is CoupMove)?"Move ":"Wall" )+  "; coup : (" + node.coup.coord[0]+ ","+ node.coup.coord[1] + ")" + "\n SCORE = "+ node.score);

                    if (current.score < node.score || current.score == Node.initialScore)
                    {
                        current.score = node.score;
                        bestCoup = node.coup;
                    }
                    player.DespawnWallWhenTesting(pair.Key, Orientation.Vertical);

                    beta = Mathf.Min(beta, node.score);
                    if (beta <= alpha)
                    {
                        Debug.Log("Min: return move = " + "(" + bestCoup.coord[0]+ ","+ bestCoup.coord[1] + ")");
                        return bestCoup;
                    }
                }
            }
        }
        return bestCoup;
    }
}