using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowBullets : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float coneAngle = 45f;
    public int numberOfBullets = 4;
    public float bulletLife = 1f;
    public float speed = 1f;
    public float damage = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootBulletsDown()
    {
        float startAngle = transform.eulerAngles.z - (coneAngle / 2) - 90;
        float angleIncrement = coneAngle / (numberOfBullets - 1);
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + (angleIncrement * i);
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
            GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, bulletRotation);
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.GetComponent<Bullet>().damage = damage;
        }
    }

    public void ShootBulletsUp()
    {
        float startAngle = transform.eulerAngles.z - (coneAngle / 2) - 270;
        float angleIncrement = coneAngle / (numberOfBullets - 1);
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = startAngle + (angleIncrement * i);
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
            GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, bulletRotation);
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.GetComponent<Bullet>().damage = damage;
        }
    }
}
