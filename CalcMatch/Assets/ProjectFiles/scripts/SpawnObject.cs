using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : Photon.MonoBehaviour
{

    public Transform spawnPoint;

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.CompareTag("BlueCard"))
        {

            //Debug.Log(Col.gameObject);
            Instantiate(Resources.Load(Col.name), spawnPoint.position, spawnPoint.rotation);
        }
    }

}
