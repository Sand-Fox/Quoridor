using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalWall : CustomWall
{
    public override bool CanSpawnHere()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        if (rightUpTile != null && !rightUpTile.directionDico[Direction.Left]) return false;
        if (rightDownTile != null && !rightDownTile.directionDico[Direction.Left]) return false;
        if (leftUpTile != null && !leftUpTile.directionDico[Direction.Right]) return false;
        if (leftDownTile != null && !leftDownTile.directionDico[Direction.Right]) return false;
        return true;
    }

    public override void SetUpOnSpawn()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        rightUpTile.directionDico[Direction.Left] = false;
        rightDownTile.directionDico[Direction.Left] = false;
        leftUpTile.directionDico[Direction.Right] = false;
        leftDownTile.directionDico[Direction.Right] = false;
    }
}
