using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class taylor_load : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeScene()
    {
        SceneManager.LoadScene("TaylorSeriesGame");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
