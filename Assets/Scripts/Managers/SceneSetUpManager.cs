using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;
using System.Collections;

public class SceneSetUpManager : MonoBehaviour
{
    public static SceneSetUpManager Instance;
    public static string playMode;
    public static string IAName1;
    public static string IAName2;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.SpawnUnits) OnSpawnUnits();
        if (newState == GameState.WaitForPlayer) OnWaitForPlayer();
    }

    private void OnSpawnUnits()
    {
        if (playMode == "Multiplayer" || playMode == "Player vs IA")
        {
            GameObject playerObject = PhotonNetwork.Instantiate("Units/Player", new Vector2(4, 4), Quaternion.identity);
            playerObject.GetComponent<SpriteRenderer>().color = ColorExtension.blue;
        }

        if (playMode == "Player vs IA")
        {
            GameObject IAObject = PhotonNetwork.Instantiate(IAName1, new Vector2(4, 4), Quaternion.identity);
            ReferenceManager.Instance.enemy = IAObject.GetComponent<BaseIA>();
        }

        if (playMode == "IA vs IA")
        {
            GameObject IAObject1 = PhotonNetwork.Instantiate(IAName1, new Vector2(4, 4), Quaternion.identity);
            ReferenceManager.Instance.player = IAObject1.GetComponent<BaseIA>();
            IAObject1.GetComponent<SpriteRenderer>().color = ColorExtension.blue;

            GameObject IAObject2 = PhotonNetwork.Instantiate(IAName2, new Vector2(4, 4), Quaternion.identity);
            ReferenceManager.Instance.enemy = IAObject2.GetComponent<BaseIA>();
        }
    }

    private void OnWaitForPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartCoroutine(CoinToss());
        }

        if (PhotonNetwork.OfflineMode) StartCoroutine(CoinToss());
    }

    private IEnumerator CoinToss()
    {
        yield return new WaitForEndOfFrame();
        GameState state = (Random.value > 0.5) ? GameState.Player1Turn : GameState.Player2Turn;
        GameManager.Instance.view.RPC("UpdateGameState", RpcTarget.All, state);
    }
}
