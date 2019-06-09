using UnityEngine;
using System.Collections;

[RequireComponent( typeof( PhotonView ) )]
public class OnClickRequestOwnership : Photon.MonoBehaviour
{

    public void OnMouseDown()
    {
       if( this.photonView.ownerId == PhotonNetwork.player.ID )
       {
            Debug.Log( "Not requesting ownership. Already mine." );
            return;
       }

       this.photonView.TransferOwnership(PhotonNetwork.player.ID);
       //this.photonView.RequestOwnership();
    }
}
