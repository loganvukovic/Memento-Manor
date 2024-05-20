using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gauntlet : MonoBehaviour
{
    private bool isOpen;
    private float timer;
    public float timeUntilOpen;

    private Animator animator;
    private BoxCollider2D collider;

    public AudioSource audioSource;
    public AudioClip unlockSound;

    public TextMeshProUGUI timerText;

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
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.FloorToInt(timer);
        }
    }

    public void StartGauntlet()
    {
        isOpen = false;
        animator.SetBool("IsOpen",  false);
        collider.enabled = true;
        timer = 0f;
        if (timerText != null)
        {
            timerText.enabled = true;
        }
    }

    void EndGauntlet()
    {
        isOpen = true;
        animator.SetBool("IsOpen", true);
        collider.enabled = false;
        timer = 0f;
        audioSource.clip = unlockSound;
        audioSource.Play();
        if (timerText != null)
        {
            timerText.enabled = false;
        }
    }

    void OnDestroy()
    {
        if (timerText != null)
        {
            timerText.enabled = false;
        }
    }
}
