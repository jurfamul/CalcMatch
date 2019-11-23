using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addDirections : MonoBehaviour
{
    public static bool clicked = true;
     GameObject button;
    public GameObject directions;
    // Start is called before the first frame update
    void Start()
    {

    }

  public void OnClick()
    {
        if (!clicked)
        {
            directions.SetActive(true);
            // button.transform.GetChild(1).gameObject.SetActive(false);
            clicked = true;
            // button.transform.GetChild(1).gameObject.SetActive(true);
            
        }
        else
        {
            directions.SetActive(false);
           
            clicked = false;
        }
    }


    public void Resume()
    {
        directions.SetActive(false);

        clicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
