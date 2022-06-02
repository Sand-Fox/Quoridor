using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PrivateRoom : MonoBehaviourPunCallbacks
{
    // Create private room
    public void CreatePrivateRoom()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("PrivateRoom");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameSolo");
    }

    // Leave room
    public void LeavePrivateRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Campagne");
    }
}
