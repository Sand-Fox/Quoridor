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
                if (neighbour == null || neighbour.occupiedPlayer != null || closed.Contains(neighbour)) continue;

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

    private CustomTile GetLowestFCostInOpen()
    {
        if(open.Count == 0)
        {
            Debug.Log("Liste vide");
            return null;
        }

        CustomTile bestTile = open[0];
        foreach(CustomTile tile in open)
        {
            if (tile.F < bestTile.F) bestTile = tile;
        }
        return bestTile;
    }
}
