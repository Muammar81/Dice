using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviourPun
{
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        var index = PhotonNetwork.IsMasterClient ? 0 : 1;
        var spawnPoint = spawnPoints[index];
        PhotonNetwork.Instantiate(dicePrefab.name, spawnPoint.position, spawnPoint.rotation);
    }
}
