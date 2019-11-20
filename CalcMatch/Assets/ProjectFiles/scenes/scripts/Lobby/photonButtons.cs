using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonButtons : Photon.MonoBehaviour
{
    public InputField joinRoomInput;
    public InputField createRoomInput;
    public photonHandler pHandler;
    public Text roomList;
    public string roomName;
    
    public void onClickJoinRoom()
    {
        pHandler.JoinOrCreateRoom();
    }

    public void onClickCreateRoom()
    {
        pHandler.createNewRoom();
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500));
        roomName = GUILayout.TextField(roomName);

        foreach (RoomInfo game in PhotonNetwork.GetRoomList()){
           if (GUILayout.Button(game.name + " " + game.playerCount + "/" + game.maxPlayers));
        }
        GUILayout.EndArea();
       
    }

    private void OnLeftRoom()
    {
        
    }
}
