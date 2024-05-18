using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public float platformLife;
    public float timeSinceSpawn;
    public float initialX;
    public float distanceMoved;


    void Start()
    {
        timeSinceSpawn = 0f;
        initialX = transform.position.x;
    }

    
    void Update()
    {
        transform.position = new Vector3(timeSinceSpawn * speed * transform.right.x + initialX, transform.position.y, transform.position.z);
        timeSinceSpawn += Time.deltaTime;
        distanceMoved = transform.position.x - initialX;
        if(timeSinceSpawn > platformLife)
        {
            Destroy(this.gameObject);
        }
    }
}
