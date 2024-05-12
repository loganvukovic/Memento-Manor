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
    private float damage;
    public PlayerAttacks playerAttack;
    private bool goingUp = false;


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
        playerAttack = player.GetComponent<PlayerAttacks>();
        direction = playerScript.direction;
        damage = playerAttack.damage;

        if(playerAttack.shootingUp == true)
        {
            goingUp = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (direction == -1)
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
        if (goingUp == false)
        {
            rb.velocity = new Vector2(direction * knifeSpeed * Time.deltaTime, 0);
        }
        else if (goingUp == true)
        {
            rb.velocity = new Vector2(0, knifeSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
