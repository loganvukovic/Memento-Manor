using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    private bool isOpen;
    private float timer;
    public float timeUntilOpen;

    private Animator animator;
    private BoxCollider2D collider;

    public AudioSource audioSource;
    public AudioClip unlockSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", true);
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        isOpen = true;
    }

    
    void Update()
    {
        if(!isOpen && timer <= timeUntilOpen)
        {
            timer += Time.deltaTime;
        }
        else if (!isOpen)
        {
            EndGauntlet();
        }
    }

    public void StartGauntlet()
    {
        isOpen = false;
        animator.SetBool("IsOpen",  false);
        collider.enabled = true;
        timer = 0f;
    }

    void EndGauntlet()
    {
        isOpen = true;
        animator.SetBool("IsOpen", true);
        collider.enabled = false;
        timer = 0f;
        audioSource.clip = unlockSound;
        audioSource.Play();
    }
}
