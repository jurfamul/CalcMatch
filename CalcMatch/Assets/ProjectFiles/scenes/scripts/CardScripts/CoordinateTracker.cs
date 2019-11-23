using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateTracker : MonoBehaviour
{
    ArrayList coordinates = new ArrayList();
    ArrayList names = new ArrayList();
    float test = .000f;

    public void increment()
    {
        test++;
    }

    public void tracker(string name, float coord)
    {
        coordinates.Add(coord);
        names.Add(name);

    }

    public float getCoord(string name)
    {
        float zValue = 0;
        int index = 0;

        index = names.IndexOf(name);

        zValue = (float)coordinates[index];

        return zValue;
    }

    public void destroyCoord(string card)
    {
        int index = names.IndexOf(card);

        names.RemoveAt(index);
        coordinates.RemoveAt(index);
    }

    public void print()
    {
        Debug.Log("TEST COORDINATE IS " + test);
    }
}
