using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 1f; //Defines how long before the bullet is destroyed
    public float rotation = 0f;
    public float speed = 1f;
    public float damage = 5f;
    public bool isWave = false;
    public float waveAmplitude;
    public float waveFrequency;
    public bool waveUpDown;
    public bool isRebound; // Added isRebound property
    public int reboundCount; // Added reboundCount property
    public float reboundDistance; // Added reboundDistance property
    public float reboundAngle; // Added reboundAngle property

    private Vector2 spawnPoint;
    private float timer = 0f;
    public bool isLob;

    private Vector2 initialVelocity; // Added initialVelocity property
    private int currentReboundCount; // Added currentReboundCount property

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
        currentReboundCount = 0; // Initialize currentReboundCount
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > bulletLife) Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = Movement(timer);

        if (isRebound && currentReboundCount < reboundCount) // Check if rebound behavior is enabled and not exceeded rebound count
        {
            float distance = Vector2.Distance(spawnPoint, transform.position);
            if (distance >= reboundDistance) // Check if distance traveled exceeds rebound distance
            {
                // Reverse direction based on rebound angle
                initialVelocity = Quaternion.Euler(0, 0, reboundAngle) * -initialVelocity;
                currentReboundCount++; // Increment rebound count
            }
        }

        transform.position = (Vector2)transform.position + initialVelocity * Time.deltaTime;
    }

    public void SetInitialVelocity(Vector2 velocity)
    {
        initialVelocity = velocity;
    }

    private Vector2 Movement(float timer)
    {
        //Moves right according to the bullet's rotation
        float x;
        float y;
        if (isWave && !waveUpDown)
        {
            x = timer * speed * transform.right.x;
            y = Mathf.Sin(x * waveFrequency) * waveAmplitude;
        }
        else if (isWave && waveUpDown)
        {
            y = timer * speed * transform.up.y;
            x = Mathf.Sin(y * waveFrequency) * waveAmplitude;
        }
        else
        {
            x = timer * speed * transform.right.x;
            y = timer * speed * transform.right.y;
        }
        return new Vector2(x + spawnPoint.x, y + spawnPoint.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.tag == "Ground")
        {
            Destroy(this.gameObject);
        }*/
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerMovement>().timeSinceDamage > other.GetComponent<PlayerMovement>().invincibleTime)
            {
                other.GetComponent<PlayerMovement>().currentHealth -= damage;
                other.GetComponent<PlayerMovement>().timeSinceDamage = 0;
            }

            Destroy(this.gameObject);
        }
    }

}
