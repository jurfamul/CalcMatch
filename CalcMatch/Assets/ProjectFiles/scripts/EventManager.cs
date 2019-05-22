using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


 public class EventManager : MonoBehaviour
{
    public Transform spawnPoint;
    public static bool ButtOn = true;
    public Button myButton;



    public void OnButtonClick()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
        {
            Instantiate(Resources.Load(go.name), spawnPoint.position, spawnPoint.rotation);
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