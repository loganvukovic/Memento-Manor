using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillsHandsBoss : MonoBehaviour
{
    public BoxCollider2D[] invisWalls;
    public Animator healthAnim;
    public BossScript bossScript;
    public AudioSource audioSource;
    public AudioClip newAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        foreach (BoxCollider2D wall in invisWalls)
        {
            wall.enabled = false;
        }
        healthAnim.SetBool("inFight", false);
        bossScript.activeBoss = false;
        audioSource.clip = newAudioClip;
        audioSource.Play();
    }
}
