using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class exit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeScene()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.playerList.Length > 1)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.playerList[1]);
            }
        }
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("DemoWorker-Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
