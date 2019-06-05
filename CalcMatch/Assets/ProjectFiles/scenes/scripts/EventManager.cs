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
    GameObject go;
    Color color;
    string cChange;
    float b;
    public Text Error;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        Error.enabled = false;
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
            cChange = go.name;
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
                    PV.RPC("RPC_disable", PhotonTargets.AllBuffered, cChange);
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
            cChange = go.name;
            butName = go.name + "(Clone)";
            Debug.Log(go.name);

            card = GameObject.Find(butName);
            card.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
            go.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
            if(card.GetPhotonView().transform.childCount >=1)
            {
                StartCoroutine(Error_Wait());
            }

            
            else
            {
                Debug.Log("no child kill it");
                PhotonNetwork.Destroy(card);
                PV.RPC("RPC_disable", PhotonTargets.AllBuffered, cChange);
            }
       
            //clicked = false;
        }
        card.GetComponent<PhotonView>().TransferOwnership(0);
        GameObject.Find(cChange).gameObject.GetComponent<PhotonView>().TransferOwnership(0);
    
    }

  [PunRPC]
    public void RPC_disable(string c)
    {
        if (clicked == true)
        {
            color = GameObject.Find(c).gameObject.GetComponent<Image>().color;
            // go.GetComponent<Image>().color = Color.gray;
            color.a = 1;
            // go.GetComponent<Image>().color = color;
            GameObject.Find(c).gameObject.GetComponent<Image>().color = color;
            Debug.Log("clicked is being set to false button name is " + c);
            //Debug.Log("SETTING TRUE");
            clicked = false;
        }
        else
        {
            color = GameObject.Find(c).gameObject.GetComponent<Image>().color;
            // color = go.gameObject.GetComponent<Image>().color;
            color.a = .5f;
            GameObject.Find(c).gameObject.GetComponent<Image>().color = color;
            Debug.Log("clicked is being set to true button name is " + c);
            //Debug.Log("Setting False");
            clicked = true;
        }

   
  }


    IEnumerator Error_Wait()
    {
        Error.enabled = true;
        yield return new WaitForSeconds(2);
        Error.enabled = false;
    }
    

  //[PunRPC]
  //public void RPC_enable()
  //{
  //  Debug.Log("SETTING FALSE");
  //  clicked = false;

  //}

}