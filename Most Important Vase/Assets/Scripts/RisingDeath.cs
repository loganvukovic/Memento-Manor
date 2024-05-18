using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RisingDeath : MonoBehaviour
{
    public float speed;
    public float platformLife;
    public float timeSinceSpawn;
    public float initialY;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceSpawn = 0f;
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, timeSinceSpawn * speed * transform.right.x + initialY, transform.position.z);
        timeSinceSpawn += Time.deltaTime;
    }
}
