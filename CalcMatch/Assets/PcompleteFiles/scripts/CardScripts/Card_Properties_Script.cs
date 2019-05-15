using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Properties_Script : MonoBehaviour
{
    public GameObject gameObject;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
