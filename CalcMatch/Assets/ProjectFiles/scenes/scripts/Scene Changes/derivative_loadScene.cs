using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class derivative_loadScene : MonoBehaviour
{
    string scene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void changeScene()
    {
       
        
        Debug.Log(scene);
        SceneManager.LoadScene("DerivativeGame");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
