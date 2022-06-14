using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomCorner : MonoBehaviour
{
    [SerializeField] private HorizontalWall horizontalWallPrefab;
    [SerializeField] private VerticalWall verticalWallPrefab;
    [SerializeField] private GameObject visual;
    public static Orientation orientation = Orientation.Horizontal;

    public bool isOpen = true;
    private CustomWall wallPreview;

    public void OnMouseEnter()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Wall || !isOpen) return;
        wallPreview = Instantiate(GetWallPrefab(), transform.position, Quaternion.identity);
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
        GameObject wallObject = PhotonNetwork.Instantiate(GetWallPrefab().name, Vector3.zero, Quaternion.identity);
        wallObject.GetComponent<CustomWall>().view.RPC("SetWall", RpcTarget.All, transform.position);
        UIManager.Instance.wallCount--;
        OnMouseExit();
    }

    private CustomWall GetWallPrefab()
    {
        if (orientation == Orientation.Horizontal) return horizontalWallPrefab;
        return verticalWallPrefab;
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
    Horizontal,
    Vertical
}
