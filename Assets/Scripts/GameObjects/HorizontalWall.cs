using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalWall : CustomWall
{
    private void Start() => orientation = Orientation.Horizontal;

    public override bool CanSpawnHere()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        if (rightUpTile != null && !rightUpTile.directionDico[Vector2.down]) return false;
        if (rightDownTile != null && !rightDownTile.directionDico[Vector2.up]) return false;
        if (leftUpTile != null && !leftUpTile.directionDico[Vector2.down]) return false;
        if (leftDownTile != null && !leftDownTile.directionDico[Vector2.up]) return false;
        return true;
    }

    public override void OnSpawn()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        rightUpTile.directionDico[Vector2.down] = false;
        rightDownTile.directionDico[Vector2.up] = false;
        leftUpTile.directionDico[Vector2.down] = false;
        leftDownTile.directionDico[Vector2.up] = false;
    }

    public override void OnDespawn()
    {
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        rightUpTile.directionDico[Vector2.down] = true;
        rightDownTile.directionDico[Vector2.up] = true;
        leftUpTile.directionDico[Vector2.down] = true;
        leftDownTile.directionDico[Vector2.up] = true;
    }
}
