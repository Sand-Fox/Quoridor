using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileExtension
{
    public static Vector2 DirectionTo(this CustomTile source, CustomTile destination)
    {
        return destination.transform.position - source.transform.position;
    }

    public static int DistanceTo(this CustomTile source, CustomTile destination)
    {
        Vector2 direction = destination.transform.position - source.transform.position;
        return (int)(Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
    }

    public static Vector2 ReflectPosition(this Vector3 position)
    {
        Vector2 ret = Vector2.Reflect(position, new Vector2(0, 1));
        return ret + new Vector2(0, 8);
    }


    public static CustomTile GetRightTile(this CustomTile centerTile)
    {
        if (!centerTile.directionDico[Direction.Right]) return null;
        Vector2 centerTilePos = centerTile.transform.position;
        return GridManager.Instance.GetTileAtPosition(centerTilePos + Vector2.right);
    }

    public static CustomTile GetLeftTile(this CustomTile centerTile)
    {
        if (!centerTile.directionDico[Direction.Left]) return null;
        Vector2 centerTilePos = centerTile.transform.position;
        return GridManager.Instance.GetTileAtPosition(centerTilePos + Vector2.left);
    }

    public static CustomTile GetUpTile(this CustomTile centerTile)
    {
        if (!centerTile.directionDico[Direction.Up]) return null;
        Vector2 centerTilePos = centerTile.transform.position;
        return GridManager.Instance.GetTileAtPosition(centerTilePos + Vector2.up);
    }

    public static CustomTile GetDownTile(this CustomTile centerTile)
    {
        if (!centerTile.directionDico[Direction.Down]) return null;
        Vector2 centerTilePos = centerTile.transform.position;
        return GridManager.Instance.GetTileAtPosition(centerTilePos + Vector2.down);
    }

    public static CustomTile[] AdjacentTiles(this CustomTile centerTile)
    {
        return new CustomTile[] { centerTile.GetRightTile(), centerTile.GetLeftTile(), centerTile.GetUpTile(), centerTile.GetDownTile() };
    }

    public static CustomTile GetFurtherTiles(this CustomTile centerTile, CustomTile adjacentTile, out CustomTile tile2)
    {
        tile2 = null;
        Vector2 d = centerTile.DirectionTo(adjacentTile);
        Direction direction = (d == Vector2.right) ? Direction.Right : (d == Vector2.left) ? Direction.Left : (d == Vector2.up) ? Direction.Up : Direction.Down;
        if (adjacentTile.directionDico[direction]) return GridManager.Instance.GetTileAtPosition((Vector2)adjacentTile.transform.position + d);

        tile2 = GridManager.Instance.GetTileAtPosition((Vector2)adjacentTile.transform.position + Vector2.Perpendicular(d));
        return GridManager.Instance.GetTileAtPosition((Vector2)adjacentTile.transform.position - Vector2.Perpendicular(d));
    }
}
