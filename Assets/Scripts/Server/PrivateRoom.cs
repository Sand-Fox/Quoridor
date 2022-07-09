using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PrivateRoom : MonoBehaviourPunCallbacks
{
    public static PrivateRoom Instance;
    private void Awake() => Instance = this;

    public void CreatePrivateRoom()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("PrivateRoom");
    }

    public override void OnJoinedRoom()
    {
        SceneSetUpManager.playMode = SceneManager.GetActiveScene().name;
        PhotonNetwork.LoadLevel("Game");
    }
}
