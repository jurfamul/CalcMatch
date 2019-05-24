using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


 public class EventManager : Photon.PunBehaviour
{
    public Transform spawnPoint;
    public static bool ButtOn = true;
    //public photonButtons myButton;
    public Button myButton;



    public void OnButtonClick()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
        {
            PhotonNetwork.Instantiate(go.name, spawnPoint.position, spawnPoint.rotation,0);
            //myButton.enabled = false;
            myButton.interactable = false;
            ButtOn = false;

           
        }
        else
        {
            Debug.Log("currentSelectedGameObject is null");
        }
    }

    //public void Update()
    //{
    //    if(ButtOn == true)
    //    {
    //        myButton.interactable = true;
    //    }
    //}
}