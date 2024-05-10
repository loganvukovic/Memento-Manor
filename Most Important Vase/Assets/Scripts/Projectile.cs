using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float knifeSpeed;
    public PlayerMovement playerScript;
    public GameObject player;
    public float direction;


    /*private void OnAwaken()
    {
        rb = GetComponent<Rigidbody2D>();

        if(playerScript.direction == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }*/

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        direction = playerScript.direction;


        if (direction == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(direction * knifeSpeed * Time.deltaTime, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
}
