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

    //Floats used to set the double click detection window for the DoubleClick implementation on line 91
    private float last_Click_Time = 0f;
    private float catch_Time = 0.25f;

    //The local vectors used to determine the position of the children of the card when the InspectGroup method is called.
    public Vector3 child0_Shift;
    public Vector3 child1_Shift;

    //The boolean used to indicate if the current card set is in viewing mode in the InspectGroup method
    private bool isViewing;

    //The boolean used to indicate if the current card set is contained with in the full-set corral
    public bool inCorral;

    private Vector3 StartingScale;
    public Vector3 NewScale;

    //The names of the card game objects that are passed it to the RPC calls
    string card1;
    string card2;

    //The photonView component that is attatched to current card. Used to make RPC calls.
    private PhotonView PV;

    //The x and y world space coordinates for the complete set boundary
    private float x_Bound = -7.27f;
    private float y_Bound = -3.86f;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        isViewing = false;
        playing_Card_Sprite = gameObject.GetComponent<SpriteRenderer>();
        StartingScale = gameObject.transform.localScale;
        NewScale = gameObject.transform.localScale / 2;
    }

    // Update is called once per frame
    void Update()
    {
        StopAtScreenBoundary();
        InspectGroup();
        RemoveChildren();
        StopAtFullSetBoundary();
        ScaleSetInBounds();
    }

    private bool MouseOverTest()
    {
        ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (ray.collider != null)
        {
            if (ray.collider.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }

    private bool DoubleClick(bool rightClick)
    {
        if (!rightClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (ray.collider != null)
                {
                    if (ray.collider.gameObject == gameObject)
                    {
                        if (Time.time - last_Click_Time < catch_Time)
                        {
                            //double click
                            print("done:" + (Time.time - last_Click_Time).ToString());
                            last_Click_Time = Time.time;
                            return true;
                        }
                        else
                        {
                            //normal click
                            print("miss:" + (Time.time - last_Click_Time).ToString());
                            last_Click_Time = Time.time;
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (ray.collider != null)
                {
                    if (ray.collider.gameObject == gameObject)
                    {
                        if (Time.time - last_Click_Time < catch_Time)
                        {
                            //double click
                            print("done:" + (Time.time - last_Click_Time).ToString());
                            last_Click_Time = Time.time;
                            return true;
                        }
                        else
                        {
                            //normal click
                            print("miss:" + (Time.time - last_Click_Time).ToString());
                            last_Click_Time = Time.time;
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        int child_Count = gameObject.GetPhotonView().transform.childCount;

        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift"))
        {
            if (MouseOverTest())
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
                            if (child_Count == 0)
                            {
                                PV.RPC("addchild_RPC", PhotonTargets.AllBuffered, card1, card2);
                            }
                            else if (child_Count > 0)
                            {
                                int child_Type = gameObject.GetPhotonView().GetComponentsInChildren<Card_Properties_Script>()[1].card_Type;
                                if (gameObject.GetPhotonView().GetComponentsInChildren<Card_Properties_Script>()[1].card_Type != other_Card_Type)
                                {
                                    PV.RPC("addchild_RPC", PhotonTargets.AllBuffered, card1, card2);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void RemoveChildren()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (MouseOverTest())
            {
                int child_Count = gameObject.GetPhotonView().transform.childCount;
                if (child_Count > 0)
                {
                    card1 = gameObject.name;
                    for (int i = child_Count - 1; i >= 0; i--)
                    {
                        PV.RPC("removeChild_RPC", PhotonTargets.AllBuffered, card1, i);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void addchild_RPC(string c1, string c2)
    {
        GameObject.Find(c1).GetPhotonView().transform.SetParent(GameObject.Find(c2).GetPhotonView().transform);
        GameObject.Find(c1).GetPhotonView().GetComponent<Collider2D>().enabled = false;
        GameObject.Find(c1).GetPhotonView().transform.localPosition = Vector3.zero;
    }

    [PunRPC]
    private void removeChild_RPC(string c1, int i)
    {

        Transform child_Transform = GameObject.Find(c1).GetPhotonView().transform.GetChild(i);
        child_Transform.SetParent(null);
        child_Transform.gameObject.GetPhotonView().GetComponent<Collider2D>().enabled = true;
    }

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
        Debug.Log("is viewing is " + isViewing);
    }

    [PunRPC]
    private void collapseGroup_RPC(string c2, int child_Count)
    {
        for (int i = child_Count - 1; i >= 0; i--)
        {
            Transform child_Transform = GameObject.Find(c2).GetPhotonView().transform.GetChild(i);
            if (i == 1)
            {
                child_Transform.localPosition = Vector3.zero;
            }
            else
            {
                child_Transform.localPosition = Vector3.zero;
            }
        }
        isViewing = false;
        Debug.Log("is viewing is " + isViewing);
    }

    private void InspectGroup()
    {
        if (Input.GetKeyDown("space"))
        {
            if (MouseOverTest())
            {
                if (!isViewing)
                {
                    card1 = gameObject.name;
                    int child_Count = gameObject.GetPhotonView().transform.childCount;
                    if (child_Count > 0)
                    {
                        PV.RPC("expandGroup_RPC", PhotonTargets.AllBuffered, card1, child_Count);
                        Debug.Log("is viewing is " + isViewing);
                    }
                }
                else
                {
                    card2 = gameObject.name;

                    int child_Count = gameObject.GetPhotonView().transform.childCount;
                    if (child_Count > 0)
                    {
                        PV.RPC("collapseGroup_RPC", PhotonTargets.AllBuffered, card2, child_Count);
                        Debug.Log("is viewing is " + isViewing);
                    }
                }
            }
        }
    }

    private void StopAtFullSetBoundary()
    {
        if (gameObject.transform.childCount != 2)
        {
            if (gameObject.transform.parent == null)
            {
                Vector3 left_Offset = this.transform.right * this.GetComponent<BoxCollider2D>().bounds.extents.x * -1f;
                Vector3 top_Offset = this.transform.up * this.GetComponent<BoxCollider2D>().bounds.extents.y;

                Vector3 current_Position = this.transform.position;
                Vector3 current_Position_Left = current_Position + left_Offset;
                Vector3 current_Position_Top = current_Position + top_Offset;

                if (current_Position_Left.x < x_Bound && current_Position.y > y_Bound)
                {
                    current_Position = new Vector3(x_Bound - left_Offset.x, current_Position.y, current_Position.z);
                    this.transform.position = current_Position;
                }
                else if (current_Position.x < x_Bound && current_Position_Top.y < y_Bound)
                {
                    current_Position = new Vector3(current_Position.x, y_Bound - top_Offset.y, current_Position.z);
                    this.transform.position = current_Position;
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

    private void ScaleSetInBounds()
    {
        if (gameObject.transform.childCount == 2)
        {
            if (gameObject.transform.parent == null)
            {
                Vector3 CurrentPosition = gameObject.GetPhotonView().transform.position;

                if (!inCorral)
                {
                    if (CurrentPosition.x < x_Bound && CurrentPosition.y > y_Bound)
                    {
                        gameObject.GetPhotonView().transform.localScale = NewScale;
                        inCorral = true;
                        Debug.Log("Entered corral");
                    }
                }
                else
                {
                    if (CurrentPosition.x > x_Bound || CurrentPosition.y < y_Bound)
                    {
                        gameObject.GetPhotonView().transform.localScale = StartingScale;
                        inCorral = false;
                        Debug.Log("Left Corral");
                    }
                }
            }
        }
    }
}