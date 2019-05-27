using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Properties_Script : MonoBehaviour
{
    public GameObject card;

    private Vector3 screenPoint;
    private Vector3 offset;

    // These are the game objects components that script will interact with during run time. This list will be updated as needed.
    public static SpriteRenderer playing_Card_Sprite;

    // These variables can be seen and modified in the unity editer in real time.
    // This integer, between 0 and 2, will represent which type of card this object represents.
    // 0 = equation, 1 = derivative, and 2 = graph.
    public int card_Type;
    // This integer will represent the set in which this card belongs.
    public int group_Num;

    private float xbottom = -0.03f;
    private float ybottom = -4.0f;

    private float xtop = -0.03f;
    private float ytop = 4.0f;

    private float xleft = -9.5f;
    private float yleft = -0.07f;

    private float xright = 9.5f;
    private float yright = 0.04f;
    private RaycastHit2D ray;

    private float last_Click_Time = 0f;
    private float catch_Time = 0.25f;

    public Vector3 child0_Shift;
    public Vector3 child1_Shift;
    private bool isViewing;


    /* TODO:
     * Collisions:
        Implement collition detection in update using rigidbodies and box_coliders to determine when cards are in contact.
        If there is less then or greater then three cards in the collision, do nothing.
        Otherwise, compare the group numbers of the cards in the collition.
        If the group number of all three cards matches, group cards into a set using collision hierarchy
        Otherwise, do nothing or have the card change color to indicate that the cards do not make up a set.

       Screen Bounds:
        Update input handler to ensure that the card can not be moved compleately outside of the range of the camera
       
       Event Listener:
        Update (or replace) existing input handler script to allow the card to listen to either the player on the local system
        or the players on the server using an event listener.

       Scene Manager:
        Implement a scene manager object that will handle scene transitions and keep global data between loads

       Network Manager:
        Implement network manager object and network player objects which will listen to the host server and translate the server
        updates into usable inputs which can be understood by the event listener.

       Pre-fabs:
        Create playing card pre-fab which can be used to quickly create new playing card objects.
     */


    // Start is called before the first frame update
    void Start()
    {
        card_Type = 0;
        group_Num = 0;
        in_Group = false;

        playing_Card = gameObject.GetComponent<SpriteRenderer>();
    }

    string butName;
    GameObject tempButton;
    Button newbutton;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            //offset = gameObject.transform.position -
            // Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (ray.collider != null)
            {
                if (ray.collider.gameObject == gameObject)
                {

                    butName = ray.transform.gameObject.name;
                    butName = butName.Substring(0, butName.Length - 7);
                    //Debug.Log(GameObject.Find(butName));
                    newbutton = GameObject.Find(butName).GetComponent<Button>();
                    newbutton.interactable = true;

                    PhotonNetwork.Destroy(gameObject);
                }

            }
            //Destroy(gameObject, .00001f);
           // Debug.Log("TEST DESTROY");

        }*/

        if (transform.position.y < ybottom)
        {
            Vector3 newPosition = new Vector3(transform.position.x, ybottom, transform.position.z);
            transform.position = newPosition;
        }
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

        InspectGroup();
        RemoveChildren();
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        int child_Count = gameObject.GetPhotonView().transform.childCount;

        // double click to add a card to the current set
        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift"))
        {
            if (MouseOverTest())
            {
                if (collision.gameObject.CompareTag("Card"))
                {
                    GameObject other_Card = collision.gameObject;
                    int other_Card_Type = other_Card.GetComponent<Card_Properties_Script>().card_Type;

                    if (other_Card_Type != card_Type)
                    {
                        if (child_Count == 0)
                        {
                            other_Card.GetPhotonView().transform.SetParent(gameObject.GetPhotonView().transform);
                            other_Card.GetPhotonView().GetComponent<Collider2D>().enabled = false;
                            other_Card.GetPhotonView().transform.localPosition = Vector3.zero;
                        }
                        else if (child_Count == 1)
                        {
                            if (gameObject.GetPhotonView().GetComponentInChildren<Card_Properties_Script>().card_Type != other_Card_Type)
                            {
                                other_Card.GetPhotonView().transform.SetParent(gameObject.GetPhotonView().transform);
                                other_Card.GetPhotonView().GetComponent<Collider2D>().enabled = false;
                                other_Card.GetPhotonView().transform.localPosition = Vector3.zero;
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
                    for (int i = child_Count - 1; i >= 0; i--)
                    {
                        Transform child_Transform = gameObject.GetPhotonView().transform.GetChild(i);
                        child_Transform.SetParent(null);
                        child_Transform.gameObject.GetPhotonView().GetComponent<Collider2D>().enabled = true;
                    }
                }
            }
        }
    }

    private void InspectGroup()
    {
        if (Input.GetKeyDown("space"))
        {
            if (MouseOverTest())
            {
                if (!isViewing)
                {
                    int child_Count = gameObject.GetPhotonView().transform.childCount;

                    if (child_Count > 0)
                    {
                        for (int i = child_Count - 1; i >= 0; i--)
                        {
                            Transform child_Transform = gameObject.GetPhotonView().transform.GetChild(i);
                            Vector3 newPos = new Vector3(1, 0, 0);
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
                }
                else
                {
                    int child_Count = gameObject.GetPhotonView().transform.childCount;

                    if (child_Count > 0)
                    {
                        for (int i = child_Count - 1; i >= 0; i--)
                        {
                            Transform child_Transform = gameObject.GetPhotonView().transform.GetChild(i);
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
                    }
                }
            }
        }
    }
}

        isViewing = false;
        playing_Card_Sprite = gameObject.GetComponent<SpriteRenderer>();
    {
        /*if (Input.GetButtonDown("Jump"))