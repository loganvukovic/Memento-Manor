using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public BossScript bossScript;
    public BoxCollider2D[] invisWalls;
    public Animator healthAnim;
    public FollowandSlam[] handScripts;
    public BurrowScript burrowScript;
    public SpriteRenderer bossSprite;
    public RisingDeath risingScript;
    public TeleportAndShoot fakeWizard;
    public TeleportAndShoot realWizard;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bossScript.PhasePush();
            bossScript.activeBoss = true;
            foreach (BoxCollider2D wall in invisWalls) 
            {
                wall.enabled = true;
            }
            healthAnim.SetBool("inFight", true);
            foreach (FollowandSlam hand in handScripts)
            {
                if(hand != null)
                {
                    hand.enabled = true;
                }
            }
            if (burrowScript != null)
            {
                burrowScript.enabled = true;   
            }
            if(bossSprite != null)
            {
                bossSprite.enabled = true;
            }
            if (risingScript != null)
            {
                risingScript.enabled = false;
            }
            if (fakeWizard != null)
            {
                fakeWizard.enabled = false;
            }
            if (realWizard != null)
            {
                realWizard.enabled = true;
            }
        }
    }
}
