using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SemisolidLedge : MonoBehaviour
{
    public BoxCollider2D collider;
    public GameObject player;
    public PlayerMovement playerScript;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        playerScript = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y <= transform.position.y && playerScript.isGrounded() == false)
        {
            collider.enabled = false;
            //UnityEngine.Debug.Log("Gone");
        }
        else if (player.transform.position.y >= transform.position.y + 0.7 || playerScript.isGrounded())
        {
            collider.enabled = true;
            //UnityEngine.Debug.Log("Solid");
        }
        if (player.transform.position.y >= transform.position.y + 0.7 && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Input.GetKey(KeyCode.Space))
        {
            collider.enabled = false;
        }
    }
}
