using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreVase : MonoBehaviour
{
    public GameObject[] goodPathObjects;
    public Breakable vaseScript;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject thing in goodPathObjects)
            {
                if (thing != null)
                {
                    Destroy(thing);
                }
            }
            if (vaseScript != null)
            {
                vaseScript.destroyable = false;
            }
        }
    }
}
