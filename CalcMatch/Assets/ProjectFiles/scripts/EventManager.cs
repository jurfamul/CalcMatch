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




        go = EventSystem.current.currentSelectedGameObject;

        Debug.Log("button name is " + cChange);
        if (clicked == false)
        {

            if (go != null)
            {
               card = PhotonNetwork.Instantiate(go.name, spawnPoint.position, spawnPoint.rotation, 0);
                card.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
                go.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
           
                if (card.GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
                {
                    
                    PV.RPC("RPC_disable", PhotonTargets.AllBuffered, cChange);
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
    public void RPC_disable(string c)
    {
        if (clicked == true)
        {
            color = GameObject.Find(c).gameObject.GetComponent<Image>().color;
            //  go.GetComponent<Image>().color = Color.gray;
            color.a = 1;
            //  go.GetComponent<Image>().color = color;
            GameObject.Find(c).gameObject.GetComponent<Image>().color = color;
            Debug.Log("clicked is being set to false button name is " + c);
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