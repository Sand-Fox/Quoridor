using UnityEngine;

public class Player : BaseUnit
{
    private void OnEnable() => ModeManager.OnModeChanged += OnModeChanged;
    private void OnDisable() => ModeManager.OnModeChanged -= OnModeChanged;

    private void OnModeChanged(Mode mode)
    {
        if (mode == Mode.Move && view.IsMine) EnableMoveZone();
    }

    private void EnableMoveZone()
    {
        foreach(CustomTile tile in occupiedTile.AdjacentTiles())
        {
            if (tile != null && tile.occupiedUnit == null) tile.EnableTarget(true);
            if (tile != null && tile.occupiedUnit != null)
            {
                CustomTile tile1 = occupiedTile.GetFurtherTiles(tile, out CustomTile tile2);
                if (tile1 != null) tile1.EnableTarget(true);
                if (tile2 != null) tile2.EnableTarget(true);
            }
        }
    }
    
}
