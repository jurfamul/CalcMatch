using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonButtons : MonoBehaviour
{
    public InputField joinRoomInput;
    public InputField createRoomInput;

    public photonHandler pHandler;
    
    public void onClickJoinRoom()
    {

        pHandler.JoinOrCreateRoom();
    }

    public void onClickCreateRoom()
    {
        pHandler.createNewRoom();
    }



    private void OnLeftRoom()
    {
        
    }
}
