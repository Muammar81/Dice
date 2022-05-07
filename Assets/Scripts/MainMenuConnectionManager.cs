using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class MainMenuConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text connectionStatus_UI;
    private object attemptsRoutine;
    private int connectionAttempts;
    private static RoomOptions roomOptions;

    private void Start()
    {
        Connect();

        static void Connect()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();

            var roomName = Environment.UserName;
            roomOptions = new RoomOptions
            {
                MaxPlayers = 2,
                IsVisible = true
            };
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected. Joining Lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby joined, Joining Room...");
        PhotonNetwork.JoinOrCreateRoom("Default", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        var playerName = $"Player {playerCount}";

        PhotonNetwork.NickName = playerName;
        Debug.Log($"Room joined as{playerName}");
        PhotonNetwork.LoadLevel(1);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionStatus_UI.text += $"\nDisconnected\nCause: {cause}";
        if (attemptsRoutine == null)
            attemptsRoutine = StartCoroutine(RetryConnection(5));
    }

    IEnumerator RetryConnection(float delay)
    {
        while (!PhotonNetwork.IsConnected)
        {
            connectionStatus_UI.text = $"Connecting attempt {++connectionAttempts}...";
            PhotonNetwork.ConnectUsingSettings();
            yield return new WaitForSeconds(delay);
        }
        attemptsRoutine = null;
    }
}
