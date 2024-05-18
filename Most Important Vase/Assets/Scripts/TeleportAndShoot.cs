using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TeleportAndShoot : MonoBehaviour
{
    public Transform[] telepoints;
    public float timeSinceTeleport = 0;
    public float teleportCooldown;
    public GameObject player;

    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    public float coneAngle = 45f;
    public int numberOfBullets = 4;
    public float bulletLife = 1f;
    public float speed = 1f;
    public float damage = 5f;

    public bool isTeleporting = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!player.GetComponent<PlayerMovement>().inDialogue)
        StartCoroutine(Teleport());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTeleporting)
        {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        isTeleporting = true;
        GetComponent<SpriteRenderer>().enabled = true;
        Shoot();
        yield return new WaitForSeconds(teleportCooldown);
        foreach (Transform point in telepoints)
        {
            transform.position = point.position;
            if (point.position.x > player.transform.position.x)
            {
                transform.localScale = new Vector3(-4, 4, 4);
            }
            else if (point.position.x < player.transform.position.x)
            {
                transform.localScale = new Vector3(4, 4, 4);
            }
            Shoot();
            yield return new WaitForSeconds(teleportCooldown);
        }
        StartCoroutine(Teleport());
    }

    private void Shoot()
    {
        UnityEngine.Debug.Log("ddddd");
        float startAngle =0;
        if (transform.position.x > player.transform.position.x)
        {
            startAngle = transform.eulerAngles.z - (coneAngle / 2) - 180;
        }
        if (transform.position.x < player.transform.position.x)
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
