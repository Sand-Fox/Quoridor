using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomTile : MonoBehaviour
{
    [HideInInspector] public BaseUnit occupiedUnit;
    [SerializeField] private VoidEventChannelSO channel;
    [SerializeField] private GameObject mouseOver;
    [SerializeField] private GameObject visual;
    public bool isTarget = false;

    public Dictionary<Vector2, bool> directionDico = new Dictionary<Vector2, bool>();

    private void Awake()
    {
        directionDico.Add(Vector2.right, true);
        directionDico.Add(Vector2.left, true);
        directionDico.Add(Vector2.up, true);
        directionDico.Add(Vector2.down, true);
    }

    private void OnMouseEnter()
    {
        if (!GameManager.Instance.isPlayerTurn()) return;

        if (ModeManager.Instance.mode == Mode.Move) mouseOver.SetActive(true);
        if(ModeManager.Instance.mode == Mode.PathFinding)
        {
            BaseUnit player = ReferenceManager.Instance.player;
            List<CustomTile> bestPath = PathFinding.Instance.GetPath(player, this);
            if (bestPath != null)
            {
                bestPath.Insert(0, player.occupiedTile);
                LinePopUp.Create(bestPath, ColorExtension.green);
            }

            if (PathFinding.Instance.debugMode) GridManager.Instance.UpdateAllTilesText();

        }
    }

    public void OnMouseExit()
    {
        if (!GameManager.Instance.isPlayerTurn()) return;

        if (ModeManager.Instance.mode == Mode.Move) mouseOver.SetActive(false);
        if (ModeManager.Instance.mode == Mode.PathFinding)
        {
            channel.RaiseEvent();
            if (PathFinding.Instance.debugMode) GridManager.Instance.ResetAllTilesText();
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.isPlayerTurn()) return;

        if (ModeManager.Instance.mode != Mode.Move || !isTarget) return;
        mouseOver.SetActive(false);
        ReferenceManager.Instance.player.view.RPC("SetUnit", Photon.Pun.RpcTarget.All, transform.position);
    }

    public void EnableVisual(bool enable)
    {
        isTarget = enable;
        visual.SetActive(enable);
    }

    //PathFinding
    public int G, H;
    public int F => G + H;
    public CustomTile previousTile;

    [SerializeField] private TextMeshProUGUI Gtext;
    [SerializeField] private TextMeshProUGUI Htext;
    [SerializeField] private TextMeshProUGUI Ftext;

    public void ResetText()
    {
        G = GridManager.MAXPATH;
        H = GridManager.MAXPATH;

        UpdateText();
    }

    public void UpdateText()
    {
        if (G == GridManager.MAXPATH) Gtext.text = "";
        else Gtext.text = G.ToString();

        if (H == GridManager.MAXPATH) Htext.text = "";
        else Htext.text = H.ToString();

        if (F == 2 * GridManager.MAXPATH) Ftext.text = "";
        else Ftext.text = F.ToString();
    }

    public int GetDistanceFromStartTile()
    {
        int flag = 0;
        CustomTile currentTile = this;
        while(currentTile.previousTile != null && flag < 100)
        {
            flag++;
            currentTile = currentTile.previousTile;
        }
        if (flag == 100) Debug.Log("Flag atteint dans GetDistanceFromStartTile");
        return flag;
    }
}
