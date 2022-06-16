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
        if(ModeManager.Instance.mode == Mode.Move) mouseOver.SetActive(true);
        if(ModeManager.Instance.mode == Mode.PathFinding)
        {
            List<CustomTile> path = PathFinding.Instance.GetPath(ReferenceManager.Instance.player.occupiedTile, this);
            if (path != null) LinePopUp.Create(path, ColorExtension.green);
        }
    }

    public void OnMouseExit()
    {
        mouseOver.SetActive(false);
        channel.RaiseEvent();
    }

    private void OnMouseDown()
    {
        if (!GameManager.Instance.isPlayerTurn() || ModeManager.Instance.mode != Mode.Move || !isTarget) return;
        mouseOver.SetActive(false);
        ReferenceManager.Instance.player.view.RPC("SetUnit", Photon.Pun.RpcTarget.All, transform.position);
    }

    public void EnableVisual(bool enable)
    {
        isTarget = enable;
        visual.SetActive(enable);
    }

    //PathFinding
    private int _G;
    public int G { get { return _G; } set
        {
            _G = value;
            if (_G == GridManager.MAXPATH || !PathFinding.Instance.debugMode)
            {
                Gtext.text = "";
                Ftext.text = "";
            }
            else
            {
                Gtext.text = _G.ToString();
                Ftext.text = F.ToString();
            }
        }
    }

    private int _H;
    public int H { get { return _H; } set
        {
            _H = value;
            if (_H == GridManager.MAXPATH || !PathFinding.Instance.debugMode)
            {
                Htext.text = "";
                Ftext.text = "";
            }
            else
            {
                Htext.text = _H.ToString();
                Ftext.text = F.ToString();
            }
        }
    }

    public int F => G + H;
    public CustomTile previousTile;

    [SerializeField] private TextMeshProUGUI Gtext;
    [SerializeField] private TextMeshProUGUI Htext;
    [SerializeField] private TextMeshProUGUI Ftext;

    public int GetDistanceFromStartTile()
    {
        int d = 0;
        CustomTile currentTile = this;
        while(currentTile.previousTile != null)
        {
            currentTile = currentTile.previousTile; d++;
        }
        return d;
    }
}
