using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HandSlamBullets : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float coneAngle = 45f;
    public int numberOfBullets = 4;
    public float bulletLife = 1f;
    public float speed = 1f;
    public float damage = 5f;

    public GameObject player;

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ShootBullets();
        }
    }*/

    public void ShootBullets()
    {
        float startAngle;
        if (player.transform.position.x < transform.position.x)
        {
            startAngle = transform.eulerAngles.z - (coneAngle / 2) - 180;
        }
        else
        {
            startAngle = transform.eulerAngles.z - (coneAngle / 2);
        }
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
