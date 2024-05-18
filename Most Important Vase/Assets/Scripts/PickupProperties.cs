using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupProperties : MonoBehaviour
{
    public string pickupType = "None";
    public int value = 0;
    public float floatValue = 0;
    public Sprite health;
    public Sprite damage;

    void Start()
    {
        if (pickupType == "Health")
        {
            GetComponent<SpriteRenderer>().sprite = health;
        }
        else if (pickupType == "Damage")
        {
            GetComponent<SpriteRenderer>().sprite = damage;
        }
    }
}
