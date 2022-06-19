using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectionToRoom : MonoBehaviourPunCallbacks
{
    //Create and Join Room
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateServerRoom()
    {
        if (createInput.text == "") return;
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinServerRoom()
    {
        if (joinInput.text == "") return;
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    //Leave Room
    public void LeaveCurrentRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (GameManager.Instance.gameState != GameState.Loose && GameManager.Instance.gameState != GameState.Win)
            GameManager.Instance.UpdateGameState(GameState.Win);
    }
}
