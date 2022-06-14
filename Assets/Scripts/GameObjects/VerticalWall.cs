using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalWall : CustomWall
{
    private Orientation _orientation = Orientation.Vertical;
    public override Orientation orientation { get => _orientation; }

    public override bool CanSpawnHere()
    {
        CustomCorner corner = GridManager.Instance.GetCornerAtPosition(transform.position);
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        if (!corner.isOpen) return false;
        if (!rightUpTile.directionDico[Vector2.left]) return false;
        if (!rightDownTile.directionDico[Vector2.left]) return false;
        if (!leftUpTile.directionDico[Vector2.right]) return false;
        if (!leftDownTile.directionDico[Vector2.right]) return false;
        return true;
    }

    public override void OnSpawn()
    {
        EnableAdjacentTiles(false);
    }

    public override void OnDespawn()
    {
        EnableAdjacentTiles(true);
    }

    private void EnableAdjacentTiles(bool enable)
    {
        CustomCorner corner = GridManager.Instance.GetCornerAtPosition(transform.position);
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        corner.isOpen = enable;
        rightUpTile.directionDico[Vector2.left] = enable;
        rightDownTile.directionDico[Vector2.left] = enable;
        leftUpTile.directionDico[Vector2.right] = enable;
        leftDownTile.directionDico[Vector2.right] = enable;
    }
}
