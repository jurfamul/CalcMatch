using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This script contains all the varibles and code for the behaviour of the playing card game objects with the exception of the click and drag movement.
 * Function list:
 *  MouseOverTest(): line 97
 *  DoubleClick(bool): line 126
 *  RemoveChildren(): line 215
 *  InspectGroup(): line 242
 *  StopAtFullSetBoundary(): line 276
 *  StopAtScreenBoundary(): line 313
 *  ScaleSetInBounds(): line 354
 *  OnTriggerStay2D(collision2D): line 396
 *  addChild_RPC(string, string): line 439
 *  removeChild_RPC(string, int): line 456
 *  expandGroup_RPC(string, int): line 471
 *  collapseGroup_RPC(string, int): line 492
 */
[RequireComponent(typeof(PhotonView))]
public class Card_Properties_Script : Photon.PunBehaviour
{
    //The vectors used to store the world location of the mouse pointer
    private Vector3 screenPoint;
    private Vector3 offset;

    // These variables can be seen and modified in the unity editer in real time.
    // This integer, between 0 and 2, will represent which type of card this object represents.
    // 0 = equation, 1 = derivative, and 2 = graph.
    public int card_Type;
    // This integer will represent the set in which this card belongs.
    // Note: This can be used if the client wants to implement a check for correct answers in the future
    public int group_Num;

    //Coordinates for main camera bounds: Used to stop cards from leaving camera view
    private float ybottom = -5.15f;
    private float ytop = 5.15f;
    private float xleft = -9.5f;
    //The x Bound of the UI field: Used to stop the player from taking cards into the UI field.
    private float xright = 5.0f;

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

    //The vectors used to store the starting and new scaling vectors of the card object. 
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

    //update functions
    //This function checks if the mouse is in contact with the box collider of a card object using a raycast
    //returns true if the mouse is hovering over a card object and false if it is not
    private bool MouseOverTest()
    {
        //This function returns a RaycastHit with the origin at the world coordinates of the mouse pointer in the direction of the zero vector.
        //This RaycastHit object contains all colliders that overlap with the mouse pointer.
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

    /*This function checks if the user double clicked on a card by recording the time of each click and compares the difference
     * between clicks with the catch_Time of 0.25 seconds.
     * 
     * Inputs: 
     * true = check for right mouse double click.
     * false = check for left mouse double click.
     * 
     * Returns true if the difference is less than the catch_Time.
     * Returns false otherwise.
     * 
     * Note to future developers: 
     * This function is not called by any other function as it would not register double clicks consistantly.
     * Reworking the function using a coroutine may help.
     */
    private bool DoubleClick(bool rightClick)
    {
        if (!rightClick)
        {
            //mouse button 0 = left mouse button
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
                            Debug.Log("done:" + (Time.time - last_Click_Time).ToString());
                            last_Click_Time = Time.time;
                            return true;
                        }
                        else
                        {
                            //normal click
                            Debug.Log("miss:" + (Time.time - last_Click_Time).ToString());
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
            //mouse button 1 = right mouse button
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


    /*
     * This function allows the player to separate cards that are currently in a set by setting the parent pointer of the child cards to null when the user
     * right clicks on the card stack.
     */
    private void RemoveChildren()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (MouseOverTest())
            {
                //This ensures that any card sets that are within the full-set curral cannot be separated
                if (gameObject.GetPhotonView().transform.localScale != NewScale)
                {
                    int child_Count = gameObject.GetPhotonView().transform.childCount;
                    if (child_Count > 0)
                    {
                        card1 = gameObject.name;
                        for (int i = child_Count - 1; i >= 0; i--)
                        {
                            PV.RPC("removeChild_RPC", PhotonTargets.AllBuffered, card1, i);
                        }
                        child_Count = gameObject.GetPhotonView().transform.childCount;
                    }
                }
            }
        }
    }

    /*
     * This function allows the players to spread out and collapse a set of cards by mousing over them and pressing the space bar.
     */
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

    /*
     * This function stops cards that are not part of a full-set from entering the full-set corral.
     */ 
    private void StopAtFullSetBoundary()
    {
        if (gameObject.transform.childCount != 2)
        {
            if (gameObject.transform.parent == null)
            {
                Vector3 left_Offset = this.transform.right * this.GetComponent<BoxCollider2D>().bounds.extents.x * -1f;
                Vector3 top_Offset = this.transform.up * this.GetComponent<BoxCollider2D>().bounds.extents.y;

                //The current_Position is the position of the center of the card object in world coordinates
                Vector3 current_Position = this.transform.position;
                //The current_Position_Left is the location of the left edge of the card object's box collider
                Vector3 current_Position_Left = current_Position + left_Offset;
                //The current_Position_Top is the location of the top edge of the card object's box collider
                Vector3 current_Position_Top = current_Position + top_Offset;

                //True if the position of the left edge of the card has passed the outer edge full-set curral and the position of the
                //top edge of the card is greater then the lower bound of the full-set curral.
                if (current_Position_Left.x < x_Bound && current_Position.y > y_Bound)
                {
                    //Sets the position of the card equal to its position before it crossed the boundary.
                    Vector3 new_Position = new Vector3(x_Bound - left_Offset.x, current_Position.y, current_Position.z);
                    this.transform.position = new_Position;
                }
                //True if the position of the left edge of the card is less then the outer edge full-set curral and the position of the
                //top edge of the card is less then the lower bound of the full-set curral.
                else if (current_Position.x < x_Bound && current_Position_Top.y < y_Bound)
                {
                    //Sets the position of the card equal to its position before it crossed the boundary.
                    Vector3 new_Position = new Vector3(current_Position.x, y_Bound - top_Offset.y, current_Position.z);
                    this.transform.position = new_Position;
                }
            }
        }
    }

    //This function uses the bounds of the card object's boxcollider to stop cards from going outside the bounds of the main camera
    private void StopAtScreenBoundary()
    {
        if (transform.parent == null)
        {
            Vector3 top_Offset = this.transform.up * this.GetComponent<BoxCollider2D>().bounds.extents.y;
            Vector3 bottom_Offset = this.transform.up * (this.GetComponent<BoxCollider2D>().bounds.extents.y * -1f);
            Vector3 left_Offset = this.transform.right * (this.GetComponent<BoxCollider2D>().bounds.extents.x * -1f);
            Vector3 right_Offset = this.transform.right * this.GetComponent<BoxCollider2D>().bounds.extents.x;

            Vector3 current_Position_Top = this.transform.position + top_Offset;
            Vector3 current_Position_Bottom = this.transform.position + bottom_Offset;
            Vector3 current_Position_Left = this.transform.position + left_Offset;
            Vector3 current_Position_Right = this.transform.position + right_Offset;

            if (current_Position_Bottom.y < ybottom)
            {
                Vector3 newPosition = new Vector3(transform.position.x, ybottom - bottom_Offset.y, transform.position.z);
                transform.position = newPosition;
            }
            if (current_Position_Top.y > ytop)
            {
                Vector3 newPosition = new Vector3(transform.position.x, ytop - top_Offset.y, transform.position.z);
                transform.position = newPosition;
            }
            if (current_Position_Left.x < xleft)
            {
                Vector3 newPosition = new Vector3(xleft - left_Offset.x, transform.position.y, transform.position.z);
                transform.position = newPosition;
            }
            if (current_Position_Right.x > xright)
            {
                Vector3 newPosition = new Vector3(xright - right_Offset.x, transform.position.y, transform.position.z);
                transform.position = newPosition;
            }
        }
    }

    /*
     * This function reduces the scale of a set of cards as long as the set is within the bounds of the full set curral and resets the scale to the original
     * value once the set leaves the curral.
     */
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

    //collision functions
    /*
    * This function allows the player to join cards of different colors together into a matched set by setting two other cards which overlap with the card the
    * player currently owns and sets them as the children of the of that card in the scene hierarchy.
    * 
    * The maximum number of children that any card can have is two, one of each other type of card.
    * 
    * Note: The behaviour of this function is somewhat inconsistant. Player often need to shift click multiple times before the function excutes properly.
    * This could be due to a combination of the frequency with which unity handles physics calculations being out of sync with the frame rate causing
    * the game to miss shift inputs and issues with the object ownership and layering over the photon network.
    */
    //called every frame when two triggers are in contact with each other
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
                    //this ensures that two seporate card sets cannot be joined together.
                    if (other_Card.transform.childCount == 0)
                    {
                        //Checks that the other card is not of the same type as the main card or that of any of its children.
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

    //RPC functions
    //The RPC function that sets the parent pointer of the card named c1 to the card named c2.
    //This function call is sent over the photon network and updates and syncs the game states of all players in the current room.
    [PunRPC]
    private void addchild_RPC(string c1, string c2)
    {
        //Moves the child card to the lowest card rendering layer dropping it below the parent card in the card set.
        GameObject.Find(c1).GetPhotonView().GetComponent<SpriteRenderer>().sortingLayerName = "Card";
        GameObject.Find(c1).GetPhotonView().transform.SetParent(GameObject.Find(c2).GetPhotonView().transform);
        //Deactives the collider of the card c1 to ensure that only the parent card can be interacted with be the players.
        //This ensures that the parent card's children move in sync with the parent card.
        GameObject.Find(c1).GetPhotonView().GetComponent<Collider2D>().enabled = false;
        //This sets the local position of the child card to zero placing the card at the same location as its parent.
        GameObject.Find(c1).GetPhotonView().transform.localPosition = Vector3.zero;
        //Moves the parent card to a higher sprite rendering layer meaning that the parent card will be at the top of the card set.
        GameObject.Find(c2).GetPhotonView().GetComponent<SpriteRenderer>().sortingLayerName = "ParentCard";
    }

    //The RPC function that separates a child of card c1.
    //This function call is sent over the photon network and updates and syncs the game states of all players in the current room.
    [PunRPC]
    private void removeChild_RPC(string c1, int i)
    {
        GameObject parent_Card = GameObject.Find(c1);
        parent_Card.GetComponent<SpriteRenderer>().sortingLayerName = "Card";
        //finds the current child in the list of c1's children.
        Transform child_Transform = parent_Card.GetPhotonView().transform.GetChild(i);
        //sets the parent of the current child to null. Removing it from c1's object hierarchy.
        child_Transform.SetParent(null);
        //reactivates the child's collider so the players can move it freely and join it with other cards.
        child_Transform.gameObject.GetPhotonView().GetComponent<Collider2D>().enabled = true;
    }

    //The RPC function that spreads out a set of cards to allow the players to easily see all the cards the stack.
    //This function call is sent over the photon network and updates and syncs the game states of all players in the current room.
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

    //The RPC function that collapses a spread out set of card.
    //This function call is sent over the photon network and updates and syncs the game states of all players in the current room.
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
}