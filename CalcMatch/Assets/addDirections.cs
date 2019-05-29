using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class addDirections : MonoBehaviour
{
    bool clicked = false;
     GameObject button;
    public Text directions;
    // Start is called before the first frame update
    void Start()
    {
        directions.enabled = false;
    }

  public void OnClick()
    {
        if (!clicked)
        {
            directions.enabled = true;
            // button.transform.GetChild(1).gameObject.SetActive(false);
            clicked = true;
            // button.transform.GetChild(1).gameObject.SetActive(true);
            
        }
        else
        {
            directions.enabled = false;
           
            clicked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
