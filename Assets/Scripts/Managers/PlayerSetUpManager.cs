using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PlayerSetUpManager : MonoBehaviour
{
    public static PlayerSetUpManager Instance;
    private string IAName = "IABestPath";

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.SpawnPlayers) OnSpawnPlayers();
        if (newState == GameState.WaitForPlayer) OnWaitForPlayer();
    }

    private void OnSpawnPlayers()
    {
        GameObject playerObject = PhotonNetwork.Instantiate("Player", new Vector2(4, 4), Quaternion.identity);
        GameManager.Instance.player = playerObject.GetComponent<Player>();
        playerObject.GetComponent<SpriteRenderer>().color = ColorExtension.blue;

        if (PhotonNetwork.OfflineMode) PhotonNetwork.Instantiate(IAName, new Vector2(4, 4), Quaternion.identity);
    }

    private void OnWaitForPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) GameManager.Instance.playerFaction = PlayerFaction.Player1;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            GameManager.Instance.playerFaction = PlayerFaction.Player2;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            CoinToss();
        }

        if (PhotonNetwork.OfflineMode) CoinToss();
    }

    private void CoinToss()
    {
        int firstPlayer = Random.Range(1, 3);
        GameState state = (firstPlayer == 1) ? GameState.Player1Turn : GameState.Player2Turn;
        GameManager.Instance.view.RPC("UpdateGameState", RpcTarget.All, state);
    }
}
