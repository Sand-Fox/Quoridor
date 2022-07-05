using System;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PhotonView view;

    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        Instance = this;
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        UpdateGameState(GameState.GenerateGrid);
        UpdateGameState(GameState.SpawnUnits);
        UpdateGameState(GameState.WaitForPlayer);
    }

    [PunRPC]
    public void UpdateGameState(GameState newState)
    {
        gameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public bool isPlayerTurn()
    {
        if ((PhotonNetwork.LocalPlayer.ActorNumber == 1 && gameState == GameState.Player1Turn)
            || (PhotonNetwork.LocalPlayer.ActorNumber == 2 && gameState == GameState.Player2Turn)) return true;

        return false;
    }

    public bool isEnemyTurn()
    {
        if ((PhotonNetwork.LocalPlayer.ActorNumber == 1 && gameState == GameState.Player2Turn)
            || (PhotonNetwork.LocalPlayer.ActorNumber == 2 && gameState == GameState.Player1Turn)) return true;

        return false;
    }

    public void EndTurn()
    {
        GameState newState = (gameState == GameState.Player1Turn) ? GameState.Player2Turn : GameState.Player1Turn;
        if (ReferenceManager.Instance.player.occupiedTile.transform.position.y == 8) newState = GameState.Win;
        if (ReferenceManager.Instance.enemy.occupiedTile.transform.position.y == 0) newState = GameState.Loose;
        UpdateGameState(newState);
    }

    public void Surrender()
    {
        UpdateGameState(GameState.Loose);
        view.RPC("UpdateGameState", RpcTarget.Others, GameState.Win);
    }
}

public enum GameState
{
    GenerateGrid = 0,
    WaitForPlayer = 1,
    SpawnUnits = 2,
    Player1Turn = 3,
    Player2Turn = 4,
    Win = 5,
    Loose = 6
}
