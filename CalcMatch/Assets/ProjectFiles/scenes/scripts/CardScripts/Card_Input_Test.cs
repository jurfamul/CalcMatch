using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Input_Test : Photon.MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 selfPos;
    public PhotonView photonView;
    public bool devTesting = false;
    public bool wait = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    [PunRPC]
    private void Update()
    {   

        if (!devTesting)
        {
            if (photonView.isMine)
            {
                
                photonView.RPC("checkInput", PhotonTargets.All);
                //checkInput();
            }
            else
            {
                
                //photonView.RPC("smoothNetMovement", PhotonTargets.All);
                //smoothNetMovement();
            }
        } 
        else
        {
            
            photonView.RPC("checkInput", PhotonTargets.All);
            //checkInput();
        }

        
    }

    [PunRPC]
    private void checkInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
            OnMouseDrag();
        }
        
    }

    [PunRPC]
    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
    }

    [PunRPC]
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.isWriting)
        {
            Debug.Log("Entered a Send");
            stream.SendNext(transform.position);
           
            //Vector3 pos = transform.localPosition;
            //stream.Serialize(ref pos);

        } else
        {
            //Vector3 pos = Vector3.zero;
            //stream.Serialize(ref pos);
            Debug.Log("Entered a Receieve");
            selfPos = (Vector3)stream.ReceiveNext();

        }
    }

    IEnumerator Example()
    {
        wait = true;
        yield return new WaitForSeconds(.02f);
        wait = false;
       
    }


    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - 
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        if (photonView.ownerId == PhotonNetwork.player.ID)
        {
            Debug.Log("Not requesting ownership. Already mine.");
            return;
        }

        photonView.TransferOwnership(PhotonNetwork.player.ID);
        //this.photonView.RequestOwnership();

    }


    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        transform.position = curPosition;
    }
    
}