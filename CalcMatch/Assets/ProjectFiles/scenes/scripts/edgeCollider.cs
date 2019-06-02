using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeCollider : MonoBehaviour
{
    private float xbottom = -0.03f;
    private float ybottom = -4.0f;

    private float xtop = -0.03f;
    private float ytop = 6.46f;

    private float xleft = -11.5f;
    private float yleft = -0.07f;

    private float xright = 11.61f;
    private float yright = 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Card"))
        {
            if (col.transform.position.y < ybottom)
            {
                Vector3 newPosition = new Vector3(col.transform.position.x, ybottom, col.transform.position.z);
                col.transform.position = newPosition;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
      

    }
}
