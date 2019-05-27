using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class EventManager : Photon.PunBehaviour
{
    public Transform spawnPoint;
    public static bool ButtOn = true;
    //public photonButtons myButton;
    public Button myButton;
    private PhotonView PV;
    public GameObject card;
    private bool clicked = false;


    private void Start()
    {
        PV = GetComponent<PhotonView>();
        //clicked = false;
    }

    string butName;
    //GameObject card;
    public void OnButtonClick()
    {
         
        var go = EventSystem.current.currentSelectedGameObject;
  
       
        if (clicked == false)
        {

            if (go != null)
            {
               card = PhotonNetwork.Instantiate(go.name, spawnPoint.position, spawnPoint.rotation, 0);
                card.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
                if (card.GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
                {
                    
                    PV.RPC("RPC_disable", PhotonTargets.AllBuffered);
                    Debug.Log("not mine owner is " + card.GetComponent<PhotonView>().ownerId);
                }
                //clicked = true;
                else
                {
                    PV.RPC("RPC_disable", PhotonTargets.AllBuffered);
                    Debug.Log("my card id is " + card.GetComponent<PhotonView>().ownerId);
                }
                //myButton.enabled = false;

            }
            else
            {
                Debug.Log("currentSelectedGameObject is null");
            }
        }
        else if (clicked == true)
        {
        
            butName = go.name + "(Clone)";
            Debug.Log(go.name);
            card = GameObject.Find(butName);
            card.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);

            PhotonNetwork.Destroy(card);
            PV.RPC("RPC_disable", PhotonTargets.AllBuffered);
            //clicked = false;
        }
    }

    [PunRPC]
    public void RPC_disable()
    {
        if (clicked == true)
        {
            //Debug.Log("SETTING TRUE");
            clicked = false;
        }
        else
        {
            //Debug.Log("Setting False");
            clicked = true;
        }


    }

    //[PunRPC]
    //public void RPC_enable()
    //{
    //    Debug.Log("SETTING FALSE");
    //    clicked = false;


    //}

}