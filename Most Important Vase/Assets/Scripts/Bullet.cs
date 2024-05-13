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

    private Vector2 spawnPoint;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > bulletLife) Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = Movement(timer);
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
            other.GetComponent<PlayerMovement>().currentHealth -= damage;
            Destroy(this.gameObject);
        }
    }
}
