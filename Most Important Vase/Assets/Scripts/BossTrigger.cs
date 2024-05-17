using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public BossScript bossScript;
    public BoxCollider2D[] invisWalls;
    public Animator healthAnim;
    public FollowandSlam[] handScripts;


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
        }
    }
}
