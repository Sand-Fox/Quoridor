using UnityEngine;

public class Player : BaseUnit
{
    private void OnEnable() => ModeManager.OnModeChanged += OnModeChanged;
    private void OnDisable() => ModeManager.OnModeChanged -= OnModeChanged;

    private void Start()
    {
        if (view.IsMine) ReferenceManager.Instance.player = this;
        else ReferenceManager.Instance.enemy = this;
    }

    private void OnModeChanged(Mode mode)
    {
        if (mode == Mode.Move && view.IsMine) EnableMoveZone();
    }

    private void EnableMoveZone()
    {
        foreach (CustomTile tile in occupiedTile.AdjacentTiles())
        {
            if (tile != null) tile.EnableVisual(true);
        }
    }
}
