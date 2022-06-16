using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance;

    private void Awake() => Instance = this;

    public bool debugMode { get; set; }
    private List<CustomTile> open;
    private List<CustomTile> closed;

    public List<CustomTile> GetPath(CustomTile startTile, CustomTile targetTile)
    {
        SetUpPath(startTile, targetTile);
        if (targetTile.previousTile == null) return null;

        List<CustomTile> path = new List<CustomTile>() { targetTile };
        CustomTile currentTile = targetTile;
        while (currentTile.previousTile != null)
        {
            currentTile = currentTile.previousTile;
            path.Add(currentTile);
        }
        path.Reverse();
        return path;
    }

    private void SetUpPath(CustomTile startTile, CustomTile targetTile)
    {
        open = new List<CustomTile>();
        closed = new List<CustomTile>();
        open.Add(startTile);

        while (true)
        {
            CustomTile current = GetLowestFCostInOpen();
            if (current == null) return;
            open.Remove(current);
            closed.Add(current);
            if(current == targetTile) return;

            foreach (CustomTile neighbour in current.AdjacentTiles())
            {
                if (neighbour == null || closed.Contains(neighbour)) continue;

                if (!open.Contains(neighbour) || current.GetDistanceFromStartTile() + 1 < neighbour.GetDistanceFromStartTile())
                {
                    neighbour.previousTile = current;
                    neighbour.G = neighbour.GetDistanceFromStartTile();
                    neighbour.H = neighbour.DistanceTo(targetTile);
                    if (!open.Contains(neighbour)) open.Add(neighbour);
                }
            }
        }
    }

    //En Cours
    public int DistanceTo(CustomTile source, CustomTile destination)
    {
        Vector2 direction = destination.transform.position - source.transform.position;
        int distance = (int)(Mathf.Abs(direction.x) + Mathf.Abs(direction.y));

        //Enlever 1

        return distance;
    }

    private CustomTile GetLowestFCostInOpen()
    {
        if(open.Count == 0) return null;

        CustomTile bestTile = open[0];
        foreach(CustomTile tile in open)
        {
            if (tile.F < bestTile.F) bestTile = tile;
            if (tile.F == bestTile.F && tile.transform.position.y < bestTile.transform.position.y) bestTile = tile;
        }
        return bestTile;
    }
}
