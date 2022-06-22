using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PlayerSetUpManager : MonoBehaviour
{
    public static PlayerSetUpManager Instance;
    public static string IAName;

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
        GameObject playerObject = PhotonNetwork.Instantiate("Units/Player", new Vector2(4, 4), Quaternion.identity);
        playerObject.GetComponent<SpriteRenderer>().color = ColorExtension.blue;

        if (PhotonNetwork.OfflineMode)
        {
            GameObject IAObject = PhotonNetwork.Instantiate(IAName, new Vector2(4, 4), Quaternion.identity);
            ReferenceManager.Instance.enemy = IAObject.GetComponent<BaseIA>();
        }
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
        GameState state = (Random.value > 0.5) ? GameState.Player1Turn : GameState.Player2Turn;
        GameManager.Instance.view.RPC("UpdateGameState", RpcTarget.All, state);
    }
}
