using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType {Straight, Spin, Cone, Aimed}

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;
    [SerializeField] private int coneBulletCount = 3;
    [SerializeField] private float coneAngle = 30f;

    private GameObject spawnedBullet;
    private float timer = 0f;

    public GameObject player;
    private float angle;
    public Vector3 aimedOffset;
    private bool canShoot = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (spawnerType == SpawnerType.Spin) transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z+1f);
        if(timer >= firingRate && canShoot)
        {
            Fire();
            timer = 0;
        }

        if (spawnerType == SpawnerType.Aimed)
        {
            Vector3 playerDirection = (player.transform.position + aimedOffset - transform.position).normalized;
            angle = Mathf.Rad2Deg * Mathf.Atan2(playerDirection.y, playerDirection.x);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void Fire()
    {
        if (bullet)
        {
            if (spawnerType == SpawnerType.Cone)
            {
                float startAngle = transform.eulerAngles.z - (coneAngle / 2);
                float angleIncrement = coneAngle / (coneBulletCount - 1);
                for (int i = 0; i < coneBulletCount; i++)
                {
                    float angle = startAngle + (angleIncrement * i);
                    Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
                    spawnedBullet = Instantiate(bullet, transform.position, bulletRotation);
                    spawnedBullet.GetComponent<Bullet>().speed = speed;
                    spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                }
            }
            else
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canShoot = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canShoot = false;
        }
    }

    /*if (bullet)
        {
            if (spawnerType == SpawnerType.Cone)
            {
                for (int i = 0; i < 3; i++)
                {
                    float angle = transform.eulerAngles.z - coneAngle / 2 + (coneAngle / 2 * i);
                    Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
                    spawnedBullet = Instantiate(bullet, transform.position, bulletRotation);
                    spawnedBullet.GetComponent<Bullet>().speed = speed;
                    spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                }
            }
            else
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            }
        }*/
}
