using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    public GameObject knife;
    private float timeSinceShot;
    public float shotCooldown;
    public float damage = 1f;
    public bool shootingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z) && CanShoot())
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                shootingUp = true;
                Instantiate(knife, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
            }
            else if (transform.localScale.x == 1)
            {
                shootingUp = false;
                Instantiate(knife, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);
            }
            else if (transform.localScale.x == -1)
            {
                shootingUp = false;
                Instantiate(knife, new Vector2(transform.position.x - 1, transform.position.y), Quaternion.identity);
            }
            timeSinceShot = 0;
        }

        timeSinceShot += Time.deltaTime;
    }

    bool CanShoot()
    {
        return timeSinceShot >= shotCooldown;
    }
}
