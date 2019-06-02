using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Full_Set_Edge_Collider : MonoBehaviour
{
    public List<Vector3> Card_Landing_Zones;

    private float xbottom = -0.03f;
    private float ybottom = -4.0f;

    private float xtop = -0.03f;
    private float ytop = 6.46f;

    private float xleft = -11.5f;
    private float yleft = -0.07f;

    private float xright = 11.61f;
    private float yright = 0.04f;

    private float x_Bound = -7.27f;
    private float y_Bound = -3.86f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Card"))
        {
            GameObject Card_Set = collision.gameObject;
            Transform Card_Set_Trans = Card_Set.transform;

            if (Card_Set_Trans.childCount != 2)
            {
                Debug.Log("incomplete set detected");
                /*if (Card_Set_Trans.position.x < x_Bound && Card_Set_Trans.position.y > y_Bound)
                {
                    Vector3 newPos = new Vector3(x_Bound, Card_Set_Trans.position.y, Card_Set_Trans.position.z);
                    Card_Set_Trans.position = newPos;
                }
                else if (Card_Set_Trans.position.x > x_Bound && Card_Set_Trans.position.y < y_Bound)
                {
                    Vector3 newPos = new Vector3(Card_Set_Trans.position.x, y_Bound, Card_Set_Trans.position.z);
                    Card_Set_Trans.position = newPos;
                }*/
            }
        }
    }
}
