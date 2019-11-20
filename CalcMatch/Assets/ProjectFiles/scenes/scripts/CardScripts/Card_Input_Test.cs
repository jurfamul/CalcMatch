using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script contains the functions and variables needed for the click and drag movement of card objects in the game scene and syncs those movements over the
 * network using a punRPC call
 * 
 * Function List:
 * checkInput_RPC(): line 50
 * OnMouseDown(): line 62
 * OnMouseDrag: line 81
 * OnMouseUP(): line 100
 */
public class Card_Input_Test : Photon.MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 selfPos;
    public PhotonView photonView;
    public bool devTesting = false;
    public bool wait = false;
    public SpriteRenderer card_Sprite;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {   
        if (!devTesting)
        {
            if (photonView.isMine)
            {
                photonView.RPC("checkInput_RPC", PhotonTargets.All);
            }
        } 
        else
        {
            photonView.RPC("checkInput_RPC", PhotonTargets.All);
        }
    }

    //Sends the OnMouseDown, OnMouseDrag, and OnMouseUp function calls over the photon network
    //This function call is sent over the photon network and updates and syncs the game states of all players in the current room.
    [PunRPC]
    private void checkInput_RPC()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
            OnMouseDrag();
            OnMouseUp();
        }
    }

    //This function is called when the left mouse button is pressed.
    //Requests ownership of a card that the mouse pointer is hovering over.
    private void OnMouseDown()
    {
        //converts the world coordinates of the card object to screen coordinates.
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - 
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (photonView.ownerId == PhotonNetwork.player.ID)
        {
            Debug.Log("Not requesting ownership. Already mine.");
            return;
        }

        //Makes the local player the owner of the card they click on in the card's photon view.
        photonView.TransferOwnership(PhotonNetwork.player.ID);
    }

    //This function sets the position of the card object equal to the position of the mouse when the left mouse button is pressed and held down while
    //the mouse pointer is in contact with the card object's box collider.
    private void OnMouseDrag()
    {
        if (photonView.ownerId == PhotonNetwork.player.ID)
        {
            //Moves the card to the top layer of the card rendering layers if the local player is the owner of object.
            card_Sprite.sortingLayerName = "OwnedCard";
        }
        //This if statement ensures that the player cannot move a card that has been attached to another card when the player creates a set.
        if (transform.parent == null)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = curPosition;
        }
    }

    //This function sets the sorting layer of the sprite renderer to Card if the card has no children or ParentCard if it has children, moving the card
    //back in the rendering layers.
    private void OnMouseUp()
    {
        if (transform.childCount == 0)
        {
            card_Sprite.sortingLayerName = "Card";
        }
        else
        {
            card_Sprite.sortingLayerName = "ParentCard";
        }
    }
}