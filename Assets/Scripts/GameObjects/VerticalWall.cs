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

        if (rightUpTile != null && !rightUpTile.directionDico[Vector2.left]) return false;
        if (rightDownTile != null && !rightDownTile.directionDico[Vector2.left]) return false;
        if (leftUpTile != null && !leftUpTile.directionDico[Vector2.right]) return false;
        if (leftDownTile != null && !leftDownTile.directionDico[Vector2.right]) return false;
        return true;
    }

    public override void OnSpawn()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        rightUpTile.directionDico[Vector2.left] = false;
        rightDownTile.directionDico[Vector2.left] = false;
        leftUpTile.directionDico[Vector2.right] = false;
        leftDownTile.directionDico[Vector2.right] = false;
    }

    public override void OnDespawn()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        rightUpTile.directionDico[Vector2.left] = true;
        rightDownTile.directionDico[Vector2.left] = true;
        leftUpTile.directionDico[Vector2.right] = true;
        leftDownTile.directionDico[Vector2.right] = true;
    }
}
