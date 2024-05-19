using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GauntletTrigger : MonoBehaviour
{
    public Gauntlet door1;
    public Gauntlet door2;
    public CircleCollider2D[] ranges;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartGauntlet()
    {
        door1.StartGauntlet();
        door2.StartGauntlet();
        foreach (Collider2D range in ranges)
        {
            UnityEngine.Debug.Log("Cool");
            range.GetComponent<CircleCollider2D>().enabled = true;
        }
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
