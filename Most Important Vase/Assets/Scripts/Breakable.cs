using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float health;
    private float playerHealth;
    public PlayerMovement playerMovement;
    public GameObject fullHealthSpawn;
    public GameObject healthMissingSpawn;
    public GameObject desperationSpawn;
    public float desperationHealth;
    public float fullHealthValue;
    public float healthMissingValue;
    public float desperationValue;
    public string fullHealthType;
    public string healthMissingType;
    public string desperationType;
    private GameObject spawnedObject;
    public bool destroyable = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if(!destroyable)
        {
            return;
        }
        health -= damage;
        if (health <= 0f)
        {
            playerHealth = playerMovement.currentHealth;
            if (playerHealth == playerMovement.maxHealth && fullHealthSpawn != null)
            {
                spawnedObject = Instantiate(fullHealthSpawn, transform.position, transform.rotation);
                spawnedObject.GetComponent<PickupProperties>().floatValue = fullHealthValue;
                spawnedObject.GetComponent<PickupProperties>().pickupType = fullHealthType;
            }
            else if (playerHealth < playerMovement.maxHealth && playerHealth > desperationHealth && healthMissingSpawn != null)
            {
                spawnedObject = Instantiate(healthMissingSpawn, transform.position, transform.rotation);
                spawnedObject.GetComponent<PickupProperties>().floatValue = healthMissingValue;
                spawnedObject.GetComponent<PickupProperties>().pickupType = healthMissingType;
            }
            else if (playerHealth <= desperationHealth && desperationSpawn != null)
            {
                spawnedObject = Instantiate(desperationSpawn, transform.position, transform.rotation);
                spawnedObject.GetComponent<PickupProperties>().floatValue = desperationValue;
                spawnedObject.GetComponent<PickupProperties>().pickupType = desperationType;
            }
            Destroy(this.gameObject);
        }
        else return;
    }
}
