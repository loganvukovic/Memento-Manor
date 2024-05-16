using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeverScript : MonoBehaviour
{
    public TilemapCollider2D floor;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            GetComponent<Animator>().SetBool("IsPulled", true);
            floor.enabled = false;
        }

    }
}
