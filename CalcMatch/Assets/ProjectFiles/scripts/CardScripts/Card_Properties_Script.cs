using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Properties_Script : MonoBehaviour
{
    public GameObject gameObject;

    private Vector3 screenPoint;
    private Vector3 offset;

    // These are the game objects components that script will interact with during run time. This list will be updated as needed.
    public static SpriteRenderer playing_Card;

    // These variables can be seen and modified in the unity editer in real time.
    // This integer, between 0 and 2, will represent which type of card this object represents.
    // 0 = equation, 1 = derivative, and 2 = graph.
    public int card_Type;
    // This integer will represent the set in which this card belongs.
    public int group_Num;
    // This bool will begin as false and will be set to true once it is in a group will all other cards in it's set.
    public bool in_Group;

    private float xbottom = -0.03f;
    private float ybottom = -4.0f;

    private float xtop = -0.03f;
    private float ytop = 4.0f;

    private float xleft = -9.5f;
    private float yleft = -0.07f;

    private float xright = 9.5f;
    private float yright = 0.04f;
    private RaycastHit2D ray;

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
        Debug.Log("card name " + playing_Card.name);
    }

    string butName;
    GameObject tempButton;
    Button newbutton;
    // Update is called once per frame
    void Update()
    {
     /*   if (Input.GetMouseButtonDown(1))
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

        }
        */
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
