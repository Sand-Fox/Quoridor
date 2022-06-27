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

    public List<CustomTile> GetWiningPath(BaseUnit unit)
    {
        CustomTile[] endRaw;
        if (unit == ReferenceManager.Instance.player) endRaw = GridManager.Instance.GetLastRaw();
        else endRaw = GridManager.Instance.GetFirstRaw();

        List<CustomTile> bestPath = new List<CustomTile>();
        int bestDistance = GridManager.MAXPATH;

        foreach (CustomTile tile in endRaw)
        {
            var path = GetPath(unit, tile);
            int distance = (path == null) ? GridManager.MAXPATH : path.Count;
            if (distance < bestDistance)
            {
                bestPath = path;
                bestDistance = distance;
            }
        }

        return bestPath;
    }

    public List<CustomTile> GetPath(BaseUnit unit, CustomTile targetTile)
    {
        if(targetTile.occupiedUnit == unit) return new List<CustomTile>();
        if (targetTile.occupiedUnit != null) return null;

        SetUpPath(unit, targetTile);
        if (targetTile.previousTile == null) return null;

        List<CustomTile> path = new List<CustomTile>();
        CustomTile currentTile = targetTile;

        int flag = 0;
        while (currentTile != unit.occupiedTile && flag < 100)
        {
            flag++;
            path.Add(currentTile);
            currentTile = currentTile.previousTile;
        }
        if (flag == 100) Debug.Log("Flag atteint dans GetPath");
        path.Reverse();
        return path;
    }

    private void SetUpPath(BaseUnit unit, CustomTile targetTile)
    {
        open = new List<CustomTile>();
        closed = new List<CustomTile>();
        unit.occupiedTile.previousTile = null;
        open.Add(unit.occupiedTile);

        int flag = 0;
        while (flag < 100)
        {
            flag++;
            CustomTile current = GetLowestFCostInOpen(unit);
            if (current == null) return;
            open.Remove(current);
            closed.Add(current);
            if(current == targetTile) return;

            foreach (CustomTile neighbour in current.AdjacentTiles())
            {
                if (closed.Contains(neighbour)) continue;

                if (!open.Contains(neighbour) || current.GetDistanceFromStartTile() + 1 < neighbour.GetDistanceFromStartTile())
                {
                    neighbour.previousTile = current;
                    neighbour.G = neighbour.GetDistanceFromStartTile();
                    neighbour.H = DistanceTo(neighbour, targetTile, unit);
                    if (!open.Contains(neighbour)) open.Add(neighbour);
                }
            }
        }
        if (flag == 100) Debug.Log("Flag atteint dans SetUpPath");
    }

    private int DistanceTo(CustomTile source, CustomTile destination, BaseUnit myUnit)
    {
        Vector2 direction = destination.transform.position - source.transform.position;
        int distance = (int)(Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
        BaseUnit otherUnit = (myUnit == ReferenceManager.Instance.player) ? ReferenceManager.Instance.enemy : ReferenceManager.Instance.player;
        if (UnitIsBetweenSourceAndDestination(otherUnit, source, destination)) distance--;
        return distance;
    }

    private bool UnitIsBetweenSourceAndDestination(BaseUnit unit, CustomTile sourceTile, CustomTile destinationTile)
    {
        Vector2 position = unit.occupiedTile.transform.position;
        Vector2 source = sourceTile.transform.position;
        Vector2 dest = destinationTile.transform.position;

        if (position.x < Mathf.Min(source.x, dest.x)) return false;
        if (position.x > Mathf.Max(source.x, dest.x)) return false;
        if (position.y < Mathf.Min(source.y, dest.y)) return false;
        if (position.y > Mathf.Max(source.y, dest.y)) return false;
        if (position.x == source.x && position.y == dest.y) return false;
        if (position.x == dest.x && position.y == source.y) return false;
        if (position == dest) return false;
        if (position == source) return false;

        return true;
    }

    private CustomTile GetLowestFCostInOpen(BaseUnit unit)
    {
        if(open.Count == 0) return null;

        CustomTile bestTile = open[0];
        foreach(CustomTile tile in open)
        {
            if (tile.F < bestTile.F) bestTile = tile;
            if (tile.F == bestTile.F && tile.transform.position.y < bestTile.transform.position.y && unit == ReferenceManager.Instance.enemy) bestTile = tile;
            if (tile.F == bestTile.F && tile.transform.position.y > bestTile.transform.position.y && unit == ReferenceManager.Instance.player) bestTile = tile;
        }
        return bestTile;
    }
}
