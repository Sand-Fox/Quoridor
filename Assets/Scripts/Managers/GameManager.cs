using System;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PhotonView view;

    public Player player;
    public PlayerFaction playerFaction;
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
        UpdateGameState(GameState.SpawnPlayers);
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
        if ((playerFaction == PlayerFaction.Player1 && gameState == GameState.Player1Turn)
            || (playerFaction == PlayerFaction.Player2 && gameState == GameState.Player2Turn)) return true;

        return false;
    }

    public void EndTurn()
    {
        GameState newState = (gameState == GameState.Player1Turn) ? GameState.Player2Turn : GameState.Player1Turn;
        UpdateGameState(newState);
    }
}

public enum GameState
{
    GenerateGrid = 0,
    WaitForPlayer = 1,
    SpawnPlayers = 2,
    Player1Turn = 3,
    Player2Turn = 4,
    Win = 5,
    Loose = 6
}

public enum PlayerFaction
{
    Player1,
    Player2
}
