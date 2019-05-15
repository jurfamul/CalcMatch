using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photonHandler : MonoBehaviour
{
    public photonButtons photonB;
    public int id;
    private void Awake()
    {
        DontDestroyOnLoad(this.transform);
    }
    public void moveScene()
    {
        PhotonNetwork.LoadLevel("Card_Scene");
    }

    public void createNewRoom()
    {
        PhotonNetwork.CreateRoom(photonB.createRoomInput.text, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(photonB.joinRoomInput.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        moveScene();
        Debug.Log("We are connected to the room");
        id = PhotonNetwork.player.ID;
        Debug.Log("player id: " + id);
    }

    
}
