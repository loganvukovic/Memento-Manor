using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseEvent : MonoBehaviour
{
    public GameObject[] goodPathObjects;


    void OnDestroy()
    {
        foreach (GameObject thing in goodPathObjects)
        {
            Destroy(thing);
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
