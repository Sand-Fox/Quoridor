using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalWall : CustomWall
{
    public override bool CanSpawnHere()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        if (rightUpTile != null && !rightUpTile.directionDico[Direction.Down]) return false;
        if (rightDownTile != null && !rightDownTile.directionDico[Direction.Up]) return false;
        if (leftUpTile != null && !leftUpTile.directionDico[Direction.Down]) return false;
        if (leftDownTile != null && !leftDownTile.directionDico[Direction.Up]) return false;
        return true;
    }

    public override void SetUpOnSpawn()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        rightUpTile.directionDico[Direction.Down] = false;
        rightDownTile.directionDico[Direction.Up] = false;
        leftUpTile.directionDico[Direction.Down] = false;
        leftDownTile.directionDico[Direction.Up] = false;
    }
}
