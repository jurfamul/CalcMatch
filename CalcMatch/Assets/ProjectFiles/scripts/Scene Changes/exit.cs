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
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("DemoWorker-Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
