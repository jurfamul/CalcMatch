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
        var go = EventSystem.current.currentSelectedGameObject;
        scene = go.ToString();
        SceneManager.LoadScene(scene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
