using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject cornersFolder;
    [SerializeField] private ButtonScript pathButton;
    [SerializeField] private ButtonScript moveButton;
    [SerializeField] private ButtonScript wallButton;
    [SerializeField] private GameObject banner;
    [SerializeField] private PanelScript optionsPanel;
    [SerializeField] private PanelScript winPanel;
    [SerializeField] private PanelScript loosePanel;

    private int _wallCount;
    public int wallCount { get { return _wallCount; } set { _wallCount = value;  wallButton.ChangeMainText("Wall (" + _wallCount + ")"); } }

    private void Awake()
    {
        Instance = this;
        ModeManager.OnModeChanged += OnModeChanged;
        GameManager.OnGameStateChanged += OnGameStateChanged;
        wallCount = 10;
    }

    private void OnDestroy()
    {
        ModeManager.OnModeChanged -= OnModeChanged;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnModeChanged(Mode newMode)
    {
        if (newMode == Mode.Wall) cornersFolder.SetActive(true);
        else cornersFolder.SetActive(false);
    }

    private void OnGameStateChanged(GameState newState)
    {
        if(newState == GameState.Player1Turn || newState == GameState.Player2Turn) banner.SetActive(false);

        if (GameManager.Instance.isPlayerTurn()) Invoke("EnableBluttons", 0.5f);
        else
        {
            pathButton.EnableButton(false);
            moveButton.EnableButton(false);
            wallButton.EnableButton(false);
        }

        if(newState == GameState.Loose)
        {
            if (optionsPanel.isEnable) optionsPanel.EnablePanel(false);
            loosePanel.EnablePanel(true);
        }
        if (newState == GameState.Win)
        {
            if (optionsPanel.isEnable) optionsPanel.EnablePanel(false);
            winPanel.EnablePanel(true);
        }
    }

    private void EnableBluttons()
    {
        pathButton.EnableButton(true);
        moveButton.EnableButton(true);
        wallButton.EnableButton(wallCount > 0);
    }
}
