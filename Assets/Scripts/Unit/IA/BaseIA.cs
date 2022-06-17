using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseIA : BaseUnit
{
    protected int wallCount = 10;
    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Player2Turn) Invoke("PlayIA", 0.5f);
    }

    protected abstract void PlayIA();

    protected List<CustomTile> GetBestPath()
    {
        CustomTile[] firstRaw = GridManager.Instance.GetFirstRaw();
        List<CustomTile> bestPath = new List<CustomTile>();
        int bestDistance = GridManager.MAXPATH;

        foreach(CustomTile tile in firstRaw)
        {
            var path = PathFinding.Instance.GetPath(this, tile);
            int distance = (path == null) ? GridManager.MAXPATH : path.Count;
            if (distance < bestDistance)
            {
                bestPath = path;
                bestDistance = distance;
            }
        }

        return bestPath;
    }

    protected List<CustomTile> GetPlayerBestPath()
    {
        CustomTile[] lastRaw = GridManager.Instance.GetLastRaw();
        List<CustomTile> bestPath = new List<CustomTile>();
        int bestDistance = GridManager.MAXPATH;

        foreach (CustomTile tile in lastRaw)
        {
            var path = PathFinding.Instance.GetPath(ReferenceManager.Instance.player, tile);
            int distance = (path == null) ? GridManager.MAXPATH : path.Count;
            if (distance < bestDistance)
            {
                bestPath = path;
                bestDistance = distance;
            }
        }

        return bestPath;
    }
}
