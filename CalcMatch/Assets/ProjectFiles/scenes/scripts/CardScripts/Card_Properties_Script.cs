using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(PhotonView))]
public class Card_Properties_Script : Photon.PunBehaviour
{
    public GameObject card;
    private Vector3 screenPoint;
    private Vector3 offset;
    // These are the game objects components that script will interact with during run time. This list will be updated as needed.
    public SpriteRenderer playing_Card_Sprite;
    // These variables can be seen and modified in the unity editer in real time.
    // This integer, between 0 and 2, will represent which type of card this object represents.
    // 0 = equation, 1 = derivative, and 2 = graph.
    public int card_Type;
    // This integer will represent the set in which this card belongs.
    public int group_Num;
    //Coordinates for main camera bounds: Used to stop cards from leaving camera view
    private float xbottom = -0.03f;
    private float ybottom = -4.0f;
    private float xtop = -0.03f;
    private float ytop = 4.0f;
    private float xleft = -9.5f;
    private float yleft = -0.07f;
    private float xright = 9.5f;
    private float yright = 0.04f;

    //RaycastHit used to detect if the mouse is in contact with the card
    private RaycastHit2D ray;


    //The local vectors used to determine the position of the children of the card when the InspectGroup method is called.
    public Vector3 child0_Shift;
    public Vector3 child1_Shift;

    //The boolean used to indicate if the current card set is in viewing mode in the InspectGroup method
    private bool isViewing;

    //The boolean used to indicate if the current card set is contained with in the full-set corral
    public bool inCorral;

    //private Vector3 StartingScale;
    //public Vector3 NewScale;

    //The names of the card game objects that are passed it to the RPC calls
    string card1;
    string card2;

    //The photonView component that is attatched to current card. Used to make RPC calls.
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        isViewing = false;
        playing_Card_Sprite = gameObject.GetComponent<SpriteRenderer>();
       //StartingScale = gameObject.transform.localScale;
       // NewScale = gameObject.transform.localScale / 2;

    }

    // Update is called once per frame
    void Update()
    {
        StopAtScreenBoundary();
        InspectGroup();
        RemoveChildren();

    }

    //detects if there are multiple cards overlapping using a ray.
    //this needs to be reworked to work more reliably.
    private bool MouseOverTestMatching()
    {
        ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (ray.collider != null)
        {
            if (ray.collider.gameObject == gameObject)
            {
                Debug.Log("CARD NAME IS: " + ray.collider.gameObject.name);
                return true;
            }
        }
        return false;
    }


    //Detects the collisions of cards
    private void OnTriggerStay2D(Collider2D collision)
    {
        int child_Count = gameObject.GetPhotonView().transform.childCount;

        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift"))
        {
                if (collision.gameObject.CompareTag("Card"))
                {
                    GameObject other_Card = collision.gameObject;
                    if (other_Card.transform.childCount == 0)
                    {
                        int other_Card_Type = other_Card.GetComponent<Card_Properties_Script>().card_Type;
                        if (other_Card_Type != card_Type)
                        {
                            card1 = other_Card.name;
                            card2 = this.gameObject.name;
                            
                            int c1ID = other_Card.GetPhotonView().viewID;
                            int c2ID = this.gameObject.GetPhotonView().viewID;

                            if (child_Count == 0)
                            {
                                PV.RPC("addchild_RPC", PhotonTargets.AllBuffered, c1ID, c2ID);
                                //PV.RPC("multiplyScale", PhotonTargets.AllBufferedViaServer, c2ID);
                        }
                            else if (child_Count > 0)
                            {
                                int child_Type = gameObject.GetPhotonView().GetComponentsInChildren<Card_Properties_Script>()[1].card_Type;
                                if (gameObject.GetPhotonView().GetComponentsInChildren<Card_Properties_Script>()[1].card_Type != other_Card_Type)
                                {
                                    PV.RPC("addchild_RPC", PhotonTargets.AllBuffered, c1ID, c2ID);
                                    //PV.RPC("multiplyScale", PhotonTargets.AllBufferedViaServer, c2ID);
                            }
                            }
                    }
                    }
                }
        }
    }

    //called to remove children from the parent
    private void RemoveChildren()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (MouseOverTestMatching())
            {
                int child_Count = gameObject.GetPhotonView().transform.childCount;
                if (child_Count > 0)
                {
                    card1 = gameObject.name;
                    int c1ID = gameObject.GetPhotonView().viewID;
                    for (int i = child_Count - 1; i >= 0; i--)
                    {
                        //PV.RPC("originalScale", PhotonTargets.AllBufferedViaServer, c1ID);
                        PV.RPC("removeChild_RPC", PhotonTargets.AllBufferedViaServer, c1ID, i);
                    }
                }
            }
        }
    }

    //called to add children
    [PunRPC]
    private void addchild_RPC(int c1, int c2)
    {

        GameObject card1 = PhotonView.Find(c1).gameObject;
        GameObject card2 = PhotonView.Find(c2).gameObject;

        card2.GetPhotonView().transform.SetParent(card1.transform,true);

        //GameObject.Find(c1).GetPhotonView().transform.SetParent(GameObject.Find(c2).GetPhotonView().transform);
       card2.GetPhotonView().GetComponent<Collider2D>().enabled = false;
       card2.GetPhotonView().transform.localPosition = Vector3.zero;
        

    }

    //second part to remove children.
    [PunRPC]
    private void removeChild_RPC(int card, int i)
    {


        GameObject[] ActiveCards = GameObject.FindGameObjectsWithTag("Card");
        int cardCount = ActiveCards.Length;

        GameObject c1 = PhotonView.Find(card).gameObject;
        Transform child_Transform = c1.GetPhotonView().transform.GetChild(i);
        child_Transform.SetParent(null);
        //Need to assign these cards z values in order to force the rendering to be the same on each client.
        child_Transform.position= new Vector3(child_Transform.position.x,child_Transform.position.y, Random.Range(.500f,.999f)+ cardCount);
        c1.transform.position = new Vector3(c1.transform.position.x, c1.transform.position.y, Random.Range(.500f, .999f));
        child_Transform.gameObject.GetPhotonView().GetComponent<Collider2D>().enabled = true;

    }

    //this is the expand function when you hit space, it toggles the exapnsion of cards.
    [PunRPC]
    private void expandGroup_RPC(string c1, int child_Count)
    {
        for (int i = child_Count - 1; i >= 0; i--)
        {
            Transform child_Transform = GameObject.Find(c1).GetPhotonView().transform.GetChild(i);
            if (i == 1)
            {
                child_Transform.localPosition = child1_Shift;
            }
            else
            {
                child_Transform.localPosition = child0_Shift;
            }
        }
        isViewing = true;
    }

    [PunRPC]
    private void collapseGroup_RPC(string c2, int child_Count)
    {
        for (int i = child_Count - 1; i >= 0; i--)
        {
            Transform child_Transform = GameObject.Find(c2).GetPhotonView().transform.GetChild(i);
            if (i == 1)
            {
                child_Transform.localPosition = new Vector3(0, 0, child_Transform.position.z);
            }
            else
            {
                child_Transform.localPosition = new Vector3(0, 0, child_Transform.position.z);
            }
        }
        isViewing = false;
        
    }

    private void InspectGroup()
    {
        if (Input.GetKeyDown("space"))
        {
            if (MouseOverTestMatching())
            {
                if (!isViewing)
                {
                    card1 = gameObject.name;
                    int child_Count = gameObject.GetPhotonView().transform.childCount;
                    if (child_Count > 0)
                    {
                        PV.RPC("expandGroup_RPC", PhotonTargets.AllBuffered, card1, child_Count);

                    }
                }
                else
                {
                    card2 = gameObject.name;

                    int child_Count = gameObject.GetPhotonView().transform.childCount;
                    if (child_Count > 0)
                    {
                        PV.RPC("collapseGroup_RPC", PhotonTargets.AllBuffered, card2, child_Count);

                    }
                }
            }
        }
    }


            
            private void StopAtScreenBoundary()
    {
        if (transform.parent == null)
        {
            if (transform.position.y < ybottom)
            {
                Vector3 newPosition = new Vector3(transform.position.x, ybottom, transform.position.z);
                transform.position = newPosition;
            }
            if (transform.position.y > ytop)
            {
                Vector3 newPosition = new Vector3(transform.position.x, ytop, transform.position.z);
                transform.position = newPosition;
            }
            if (transform.position.x < xleft)
            {
                Vector3 newPosition = new Vector3(xleft, transform.position.y, transform.position.z);
                transform.position = newPosition;
            }
            if (transform.position.x > xright)
            {
                Vector3 newPosition = new Vector3(xright, transform.position.y, transform.position.z);
                transform.position = newPosition;
            }
        }
    }

    //Scale functions that has issues over the internet
    //[PunRPC]
    //private void multiplyScale(int g)
    //{
    //    GameObject parent = PhotonView.Find(g).gameObject.transform.root.gameObject;
    //    GameObject child = PhotonView.Find(g).gameObject;

    //    if(child.transform.childCount == 1)
    //    {
    //        GameObject temp = child.transform.GetChild(0).gameObject;
    //        temp.transform.SetParent(parent.transform);
    //    }


    //    if (parent.GetPhotonView().transform.childCount ==2 )
    //    {
    //        parent.GetPhotonView().transform.localScale = NewScale;
    //        //child.transform.localScale = NewScale;
    //    }

    //}

    //[PunRPC]
    //private void originalScale(int g)
    //{
    //    GameObject parent = PhotonView.Find(g).gameObject.transform.root.gameObject;
    //    GameObject child = PhotonView.Find(g).gameObject;

    //    if (parent.GetPhotonView().transform.childCount == 2 )
    //    {
    //        parent.GetPhotonView().transform.localScale = StartingScale;

    //    }

    //}


}