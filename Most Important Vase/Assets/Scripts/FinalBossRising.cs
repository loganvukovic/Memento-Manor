using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossRising : MonoBehaviour
{
    public RisingDeath risingScript;
    public TeleportAndShoot teleportScript;
    public SpriteRenderer wizard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            risingScript.enabled = true;
            teleportScript.enabled = true;
            wizard.enabled = false;
        }
    }
}
