using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public CustomTile occupiedTile { get; private set; }
    public PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        CustomTile tile = GridManager.Instance.GetTileAtPosition(new Vector2(4, 0));
        if (!view.IsMine) tile = GridManager.Instance.GetTileAtPosition(new Vector2(4, 8));
        tile.occupiedPlayer = this;
        occupiedTile = tile;
        transform.position = tile.transform.position;
    }

    [PunRPC]
    public void SetUnit(Vector3 position)
    {
        CustomTile tile = GridManager.Instance.GetTileAtPosition(position);
        if (!view.IsMine) tile = GridManager.Instance.GetTileAtPosition(position.ReflectPosition());

        if (tile.occupiedPlayer != null) Debug.LogWarning("Attention, il y a déjà un Unit sur cette case.");

        occupiedTile.occupiedPlayer = null;
        tile.occupiedPlayer = this;
        occupiedTile = tile;
        transform.DOMove(tile.transform.position, 0.4f).SetEase(Ease.InOutSine);

        GameManager.Instance.EndTurn();
        if (view.IsMine && tile.transform.position.y == 8) GameManager.Instance.UpdateGameState(GameState.Win);
        if (!view.IsMine && tile.transform.position.y == 0) GameManager.Instance.UpdateGameState(GameState.Loose);
    }

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
            if (tile != null && tile.occupiedPlayer == null) tile.EnableTarget(true);
            if (tile != null && tile.occupiedPlayer != null)
            {
                CustomTile tile1 = occupiedTile.GetFurtherTiles(tile, out CustomTile tile2);
                if (tile1 != null) tile1.EnableTarget(true);
                if (tile2 != null) tile2.EnableTarget(true);
            }
        }
    }
    
}
