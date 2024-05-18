using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    public GameObject[] objectsToDestroy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            foreach (GameObject thing in objectsToDestroy)
            {
                if (thing != null)
                {
                    Destroy(thing);
                }
            }
        }
    }
}
