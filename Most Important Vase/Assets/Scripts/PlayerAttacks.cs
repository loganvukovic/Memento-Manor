using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    public GameObject knife;
    private float timeSinceShot;
    public float shotCooldown;
    public float damage = 1f;
    public bool shootingUp = false;
    public bool inDialogue;
    public AudioSource audioSource;
    public AudioClip knifeSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inDialogue = GetComponent<PlayerMovement>().inDialogue;

        if (Input.GetKey(KeyCode.Z) && CanShoot() && !inDialogue)
        {
            UnityEngine.Debug.Log("Y no workey");
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                shootingUp = true;
                Instantiate(knife, new Vector2(transform.position.x, transform.position.y + 3), Quaternion.identity);
            }
            else if (Mathf.Sign(transform.localScale.x) == 1)
            {
                shootingUp = false;
                Instantiate(knife, new Vector2(transform.position.x + 1, transform.position.y + 2), Quaternion.identity);
            }
            else if (Mathf.Sign(transform.localScale.x) == -1)
            {
                shootingUp = false;
                Instantiate(knife, new Vector2(transform.position.x - 1, transform.position.y + 2), Quaternion.identity);
            }
            timeSinceShot = 0;
            audioSource.clip = knifeSound;
            audioSource.Play();
        }

        timeSinceShot += Time.deltaTime;
    }

    bool CanShoot()
    {
        return timeSinceShot >= shotCooldown;
    }
}
