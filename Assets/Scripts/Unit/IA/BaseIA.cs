using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseIA : BaseUnit
{
    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Player2Turn) Invoke("PlayIA", 0.5f);
    }

    protected abstract void PlayIA();
}
