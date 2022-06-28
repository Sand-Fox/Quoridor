using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalWall : CustomWall
{
    private Orientation _orientation = Orientation.Horizontal;
    public override Orientation orientation { get => _orientation; }

    public override bool CanSpawnHere()
    {
        CustomCorner corner = GridManager.Instance.GetCornerAtPosition(transform.position);
        CustomTile rightUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, 0.5f));
        CustomTile rightDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(0.5f, -0.5f));
        CustomTile leftUpTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, 0.5f));
        CustomTile leftDownTile = GridManager.Instance.GetTileAtPosition(transform.position + new Vector3(-0.5f, -0.5f));

        if (!corner.isOpen) return false;
        if (!rightUpTile.directionDico[Vector2.down]) return false;
        if (!rightDownTile.directionDico[Vector2.up]) return false;
        if (!leftUpTile.directionDico[Vector2.down]) return false;
        if (!leftDownTile.directionDico[Vector2.up]) return false;

        OnSpawn();

        if (!PathFinding.Instance.existPath(ReferenceManager.Instance.enemy)) 
        {
            OnDespawn();
            return false;
        }
        if (!PathFinding.Instance.existPath(ReferenceManager.Instance.player))
        {
            OnDespawn();
            return false;
        } 
        
        OnDespawn();
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
        rightUpTile.directionDico[Vector2.down] = enable;
        rightDownTile.directionDico[Vector2.up] = enable;
        leftUpTile.directionDico[Vector2.down] = enable;
        leftDownTile.directionDico[Vector2.up] = enable;
    }
}
