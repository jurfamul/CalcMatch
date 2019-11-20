using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(PhotonView))]
public class EventManager : Photon.PunBehaviour
{
    //defines where the card prefabs spawn
    public Transform spawnPoint;
    //PhotonView needed for RPC calls
    private PhotonView PV;
    //GameObject references for the card
    public GameObject card;
    //determines if we can spawn or delete a card.
    private bool clicked = false;
    //A gameobject to hold the button reference
    GameObject go;
    //Buttons color to change the transparency on the buttons
    Color color;
    //String for RPC call because you cant pass in objects.
    string cChange;
    //Holds the error message when a the trying to delete the parent.
    public Text Error;

    //Initializes photonview and error.enabled.
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        Error.enabled = false;
    }

    //holds a reference to the button name so we can find the proper prefab and spawn it.
    string butName;
    //Controls spawning of card prefabs and button disabling/coloring.
    public void OnButtonClick()
    {

        //Gets the current button that is clicked as a gameobject.
        go = EventSystem.current.currentSelectedGameObject;

        //Checks if the button isn't greyed out(ready to spawn a prefab)
        //and if the name of the button is not null
        //Also sets cChange to the button name.
        if (clicked == false)
        {
            cChange = go.name;
            if (go != null)
            {

                //Spawns appropriate card prefab based on go.name by searching the resources folder.
                //Spawns the card at the spawn point. Then transfers ownership of the button and the card
                //to the player that pressed the button.
                card = PhotonNetwork.Instantiate(go.name, spawnPoint.position, spawnPoint.rotation, 0);
                card.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
                go.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);

                //Checks if we were successful in setting the owner. Either way we call an RPC call
                //Make RPC call with the name of the method we are calling first, PhotonTargets.AllBuffered 
                //syncs to all clients and clients that connect later, and cChange is a parameter that holds
                //the card name and gets passed to "RPC_disable".
                if (card.GetComponent<PhotonView>().ownerId != PhotonNetwork.player.ID)
                {

                    PV.RPC("RPC_disable", PhotonTargets.AllBuffered, cChange);

                }
                else
                {
                    PV.RPC("RPC_disable", PhotonTargets.AllBuffered, cChange);

                }

            }
            else
            {
                //do nothing
            }
        }

        //Checks if the card has been spawned already, thus the button is disabled.
        else if (clicked == true)
        {
            //Pulls in the buttons name
            cChange = go.name;
            //Appends "(Clone)" to the button name because this should now be the 
            //prefab name that is spawned on the canvas. 
            butName = go.name + "(Clone)";

            //Find the game object that has the go.name + "(Clone)"
            card = GameObject.Find(butName);

            //transfer ownership of the button pressed and card that is going to be removed.
            card.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
            go.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);

            //Checks if the card has a child
            //if so we throw an error and do not delete the card
            //because if we did then we would delete all cards that are children.
            if (card.GetPhotonView().transform.childCount >= 1)
            {
                StartCoroutine(Error_Wait());
            }

            //destroys the card since it has no children
            //calls RPC call to enable button
            else
            {
                PhotonNetwork.Destroy(card);
                PV.RPC("RPC_disable", PhotonTargets.AllBuffered, cChange);
            }

        }
        //transfer cards and buttons ownership to the scene
        card.GetComponent<PhotonView>().TransferOwnership(0);
        GameObject.Find(cChange).gameObject.GetComponent<PhotonView>().TransferOwnership(0);

    }

    //must apply [PunRPC] attribute to the function per RPC documentation 
    //takes a string as a paramter because RPC can not take in game objects

    [PunRPC]
    public void RPC_disable(string c)
    {
        //if the button has spawned a card and is grayed out
        //sets color equal to the button color and sets the alpha value to 1
        //alpha as 1 is fully visible
        //sets clicked to false
        if (clicked == true)
        {
            color = GameObject.Find(c).gameObject.GetComponent<Image>().color;
            color.a = 1;
            GameObject.Find(c).gameObject.GetComponent<Image>().color = color;
            clicked = false;
        }
        //if clicked is not true
        //sets color to the buttons color and sets the alpha value to .5 which is half transparent
        //sets clicked to true
        else
        {
            color = GameObject.Find(c).gameObject.GetComponent<Image>().color;
            color.a = .5f;
            GameObject.Find(c).gameObject.GetComponent<Image>().color = color;
            Debug.Log("clicked is being set to true button name is " + c);
            clicked = true;
        }


    }

    //coroutine that is triggered when a deletion of a card with a child is attempted
    //sets the error message to true then waits 2 seconds and disables the error
    IEnumerator Error_Wait()
    {
        Error.enabled = true;
        yield return new WaitForSeconds(2);
        Error.enabled = false;
    }
}