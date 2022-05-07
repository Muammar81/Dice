using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelConnectionManager : MonoBehaviourPunCallbacks
{
    private Coroutine attemptsRoutine;
    private int connectionAttempts;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
            return;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) => 
        Debug.Log($"{newPlayer.NickName} joined. Total in room:{PhotonNetwork.CurrentRoom.PlayerCount}");

    public override void OnPlayerLeftRoom(Player otherPlayer) => 
        Debug.Log($"{otherPlayer.NickName} left. Total in room:{PhotonNetwork.CurrentRoom.PlayerCount}");

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected\nCause: {cause}");

        if (attemptsRoutine == null)
        {
            attemptsRoutine = StartCoroutine(RetryConnection(5));
        }
    }

    private IEnumerator RetryConnection(float delay)
    {
        while (!PhotonNetwork.IsConnected)
        {
            Debug.Log($"Connecting attempt {++connectionAttempts}...");
            PhotonNetwork.ConnectUsingSettings();
            yield return new WaitForSeconds(delay);
        }
        attemptsRoutine = null;
    }
}