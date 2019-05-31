using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    /*
    public virtual string OnJoinedLobby()
    {
        string ret = '';
        List<RoomInfo> roomList = PhotonNetwork.GetRoomList().ToList();
        Debug.Log(roomList.Count);

        foreach (RoomInfo game in PhotonNetwork.GetRoomList())
        {
            game.name + " " + game.playerCount + "/" + game.maxPlayers;
        }

        Debug.Log("room length : " + PhotonNetwork.GetRoomList().Length);
        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = 20 };
        Debug.Log("Create Room");
        return ret;
    }
    */
}
