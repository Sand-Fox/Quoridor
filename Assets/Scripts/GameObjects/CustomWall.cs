using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class CustomWall : MonoBehaviour
{
    public PhotonView view;

    [SerializeField] private SpriteRenderer wall1;
    [SerializeField] private SpriteRenderer wall2;

    private void Awake() => view = GetComponent<PhotonView>();

    public void SetColor(Color color)
    {
        wall1.color = color;
        wall2.color = color;
    }

    public void SetUpPreview()
    {
        if (CanSpawnHere()) SetColor(ColorExtension.transparent);
        else SetColor(ColorExtension.transparentRed);
    }

    [PunRPC]
    public void SetWall(Vector3 position)
    {
        CustomCorner corner = GridManager.Instance.GetCornerAtPosition(position);
        if (!view.IsMine) corner = GridManager.Instance.GetCornerAtPosition(position.ReflectPosition());
        transform.position = corner.transform.position;
        corner.gameObject.SetActive(false);
        SetUpOnSpawn();
        GameManager.Instance.EndTurn();

        bool orientation = (this is HorizontalWall)?true:false;
        Coup c = new Coup("wall", position, orientation);
        RegisterManager.Instance.AddCoup(c);
    }

    public abstract bool CanSpawnHere();
    public abstract void SetUpOnSpawn();
}
