using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance;

    public Mode mode = Mode.Normal;
    public static event Action<Mode> OnModeChanged;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Player1Turn || newState == GameState.Player2Turn) UpdateMode(Mode.Normal);
    }

    public void UpdateMode(Mode newMode)
    {
        mode = newMode;
        OnModeChanged?.Invoke(newMode);
    }

    public void UpdateModeFromButton(int newMode)
    {
        UpdateMode((Mode)newMode);
    }
}

public enum Mode
{
    Normal = 0,
    Move = 1,
    Wall = 2,
    PathFinding = 3
}
