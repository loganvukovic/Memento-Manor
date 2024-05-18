using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public float speed;
    public float platformLife;
    public float timeInBetweenSpawn;
    public float timer;

    public GameObject platform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeInBetweenSpawn)
        {
            SpawnPlatform();
            timer = 0;
        }
    }

    public void SpawnPlatform()
    {
        GameObject spawnedPlatform = Instantiate(platform, transform.position, transform.rotation);
        spawnedPlatform.GetComponent<MovingPlatform>().speed = speed;
        spawnedPlatform.GetComponent<MovingPlatform>().platformLife = platformLife;
    }
}
