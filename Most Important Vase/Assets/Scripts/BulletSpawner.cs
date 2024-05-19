using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin, Cone, Aimed, Wave, Circle, DoubleSpin, Lob, Rebound } // Added Rebound

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;
    public float damage = 5f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    public int totalShots;
    public int shotsLeft;
    [SerializeField] private float firingRate = 1f;
    public float spinSpeed;
    [SerializeField] private int coneBulletCount = 3;
    [SerializeField] private float coneAngle = 30f;
    public float waveAmplitude;
    public float waveFrequency;
    public bool waveUpDown;
    public int bulletsInCircle;

    [Header("DoubleSpin Attributes")]
    public float doubleSpinAngle = 45f;

    [Header("Lob Attributes")] // Added header
    public Vector2 lobDirection = new Vector2(1f, 1f).normalized; // Direction of the lob
    public float lobHeight = 5f; // Height of the lob

    [Header("Rebound Attributes")] // Added header
    public int reboundCount = 3; // Number of rebounds
    public float reboundDistance = 5f; // Distance traveled before rebounding
    public float reboundAngle = 0f; // Angle at which the shot comes back

    private GameObject spawnedBullet;
    private float timer = 0f;

    public GameObject player;
    private float angle;
    public Vector3 aimedOffset;
    private bool canShoot = false;

    void Start()
    {
        shotsLeft = totalShots;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (spawnerType == SpawnerType.Spin || spawnerType == SpawnerType.DoubleSpin) // Updated to include DoubleSpin
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + spinSpeed);
        }

        if (timer >= firingRate && canShoot && player.GetComponent<PlayerMovement>().inDialogue == false)
        {
            if (totalShots == 0)
            {
                Fire();
                timer = 0;
            }
            else if (totalShots > 0 && shotsLeft > 0)
            {
                Fire();
                timer = 0;
                shotsLeft -= 1;
            }
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
                    spawnedBullet.GetComponent<Bullet>().damage = damage;
                }
            }
            else if (spawnerType == SpawnerType.Circle)
            {
                float angleIncrement = 360f / bulletsInCircle;
                for (int i = 0; i < bulletsInCircle; i++)
                {
                    float angle = angleIncrement * i;
                    Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle);
                    spawnedBullet = Instantiate(bullet, transform.position, bulletRotation);
                    spawnedBullet.GetComponent<Bullet>().speed = speed;
                    spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                    spawnedBullet.GetComponent<Bullet>().damage = damage;
                }
            }
            else if (spawnerType == SpawnerType.DoubleSpin)
            {
                Quaternion bulletRotation1 = Quaternion.Euler(0f, 0f, transform.eulerAngles.z - doubleSpinAngle / 2);
                Quaternion bulletRotation2 = Quaternion.Euler(0f, 0f, transform.eulerAngles.z + doubleSpinAngle / 2);

                spawnedBullet = Instantiate(bullet, transform.position, bulletRotation1);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;

                spawnedBullet = Instantiate(bullet, transform.position, bulletRotation2);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
            }
            else if (spawnerType == SpawnerType.Lob) // Added Lob case
            {
                Vector2 startPos = transform.position;

                float lobTime = Mathf.Sqrt(2f * lobHeight / Physics2D.gravity.magnitude);
                float lobSpeedX = (lobDirection.normalized * speed).x;
                float lobSpeedY = lobTime * Physics2D.gravity.magnitude;

                for (int i = 0; i < 3; i++)
                {
                    float offsetX = i - 1;
                    Vector2 offset = new Vector2(offsetX, 0f).normalized * 0.5f;
                    Vector2 initialVelocity = new Vector2(lobSpeedX, lobSpeedY) + offset;

                    spawnedBullet = Instantiate(bullet, startPos, Quaternion.identity);
                    Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();
                    bulletScript.speed = speed; // Set speed
                    bulletScript.bulletLife = bulletLife; // Set bulletLife
                    bulletScript.damage = damage; // Set damage
                }
            }
            else if (spawnerType == SpawnerType.Rebound) // Added Rebound case
            {
                Vector2 startPos = transform.position;
                Vector2 direction = transform.right;

                spawnedBullet = Instantiate(bullet, startPos, Quaternion.identity);
                Bullet bulletScript = spawnedBullet.GetComponent<Bullet>();

                bulletScript.speed = speed;
                bulletScript.bulletLife = bulletLife;
                bulletScript.damage = damage;
                bulletScript.isRebound = true; // Set rebound behavior
                bulletScript.reboundCount = reboundCount; // Set rebound count
                bulletScript.reboundDistance = reboundDistance; // Set rebound distance
                bulletScript.reboundAngle = reboundAngle; // Set rebound angle
                bulletScript.SetInitialVelocity(direction * speed); // Set initial velocity
            }
            else
            {
                spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
                spawnedBullet.GetComponent<Bullet>().speed = speed;
                spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
                spawnedBullet.GetComponent<Bullet>().damage = damage;
                if (spawnerType == SpawnerType.Wave)
                {
                    spawnedBullet.GetComponent<Bullet>().isWave = true;
                    spawnedBullet.GetComponent<Bullet>().waveAmplitude = waveAmplitude;
                    spawnedBullet.GetComponent<Bullet>().waveFrequency = waveFrequency;
                    if (waveUpDown)
                    {
                        spawnedBullet.GetComponent<Bullet>().waveUpDown = true;
                    }
                }
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
}
