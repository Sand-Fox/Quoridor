using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class CustomWall : MonoBehaviour
{
    public PhotonView view;
    public abstract Orientation orientation { get; }

    [SerializeField] private SpriteRenderer wall1;
    [SerializeField] private SpriteRenderer wall2;

    private void Awake() => view = GetComponent<PhotonView>();

    public void SetUpPreview()
    {
        if (CanSpawnHere())
        {
            wall1.color = ColorExtension.transparent;
            wall2.color = ColorExtension.transparent;
        }
        else
        {
            wall1.color = ColorExtension.transparentRed;
            wall2.color = ColorExtension.transparentRed;
        }
    }

    [PunRPC]
    public void SetWall(Vector3 position)
    {
        if (!view.IsMine) position = position.ReflectPosition();
        transform.position = position;
        OnSpawn();
        GameManager.Instance.EndTurn();

        Orientation orientation = (this is HorizontalWall) ? Orientation.Horizontal : Orientation.Vertical;
        Coup c = new Coup("wall", position, orientation);
        RegisterManager.Instance.AddCoup(c);
    }

    public abstract bool CanSpawnHere();
    public abstract void OnSpawn();
    public abstract void OnDespawn();
}
