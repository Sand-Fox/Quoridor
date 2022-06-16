using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomCorner : MonoBehaviour
{
    [SerializeField] private GameObject visual;
    public static Orientation orientation = Orientation.Horizontal;

    public bool isOpen = true;
    private CustomWall wallPreview;

    public void OnMouseEnter()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall || !isOpen) return;
        CustomWall prefab;
        if (orientation == Orientation.Horizontal) prefab = ReferenceManager.Instance.horizontalWallPrefab;
        else prefab = ReferenceManager.Instance.verticalWallPrefab;

        wallPreview = Instantiate(prefab, transform.position, Quaternion.identity);
        wallPreview.SetUpPreview();
        GridManager.Instance.selectedCorner = this;
    }

    public void OnMouseExit()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall || !isOpen) return;
        Destroy(wallPreview.gameObject);
        GridManager.Instance.selectedCorner = null;
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall || !isOpen) return;
        if (!wallPreview.CanSpawnHere()) return;
        GameObject wallObject = PhotonNetwork.Instantiate("Wall/" + orientation + "Wall", Vector3.zero, Quaternion.identity);
        wallObject.GetComponent<CustomWall>().view.RPC("SetWall", RpcTarget.All, transform.position);
        UIManager.Instance.wallCount--;
        OnMouseExit();
    }

    public static void SwitchOrientation()
    {
        orientation = (orientation == Orientation.Horizontal) ? Orientation.Vertical : Orientation.Horizontal;
    }

    public void EnableVisual(bool enable)
    {
        visual.SetActive(enable);
    }
}

public enum Orientation
{
    None,
    Horizontal,
    Vertical
}
