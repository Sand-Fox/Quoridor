using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public abstract class BaseUnit : MonoBehaviour
{
    public CustomTile occupiedTile { get; private set; }
    public PhotonView view { get; private set; }

    private void Awake()
    {
        view = GetComponent<PhotonView>();

        CustomTile tile = GridManager.Instance.GetTileAtPosition(new Vector2(4, 0));
        if (!view.IsMine || this is BaseIA) tile = GridManager.Instance.GetTileAtPosition(new Vector2(4, 8));
        tile.occupiedUnit = this;
        occupiedTile = tile;
        transform.position = tile.transform.position;
    }

    [PunRPC]
    public void SetUnit(Vector3 position)
    {
        CustomTile tile = GridManager.Instance.GetTileAtPosition(position);
        if (!view.IsMine) tile = GridManager.Instance.GetTileAtPosition(position.ReflectPosition());

        if (tile.occupiedUnit != null) Debug.LogWarning("Attention, il y a déjà un Unit sur cette case.");

        occupiedTile.occupiedUnit = null;
        tile.occupiedUnit = this;
        occupiedTile = tile;
        transform.DOMove(tile.transform.position, 0.4f).SetEase(Ease.InOutSine);

        GameManager.Instance.EndTurn();

        CoupMove c = new CoupMove(position);
        RegisterManager.Instance.AddCoup(c);
    }
}
