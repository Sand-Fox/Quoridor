using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseIA : BaseUnit
{
    private void OnEnable() => GameManager.OnGameStateChanged += OnGameStateChanged;
    private void OnDisable() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (this == ReferenceManager.Instance.player && GameManager.Instance.isPlayerTurn()
            || (this == ReferenceManager.Instance.enemy && GameManager.Instance.isEnemyTurn()))
            Invoke("PlayIA", movementDuration);
    }

    protected abstract void PlayIA();
}
