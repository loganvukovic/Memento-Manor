using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private CapsuleCollider2D polygonCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    public float direction;
    private Vector3 correctScale;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;

    public float maxHealth;
    public float currentHealth;

    public bool inDialogue = false;
    public DialogueManager dialogueManager;
    public float dialogueCooldown = 0.7f;
    private float dialogueTimer;
    public float timeSinceDialogue;

    public GameObject camera;

    public Animator interactAnimator;
    public Animator talkAnimator;

    public GameObject permanentDialogue;

    public float invincibleTime;
    public float timeSinceDamage;

    //CharChanges

    public Transform groundCheck;
    private float jumpForceTime = 0f;
    public bool canJump;
    public bool isJumping = false;

    [SerializeField] private float maxFallSpeed = 1.5f;

    private Vector3 currentSpawn;

    public UnityEngine.UI.Image healthBar;

    public GameManager gameManager;

    public AudioSource audioSource;
    public AudioClip dashSound;

    private void Awake()
    {
        //Grab references for rigidbody
        body = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<CapsuleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Application.targetFrameRate = 60;
        currentHealth = maxHealth;
        correctScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);
        /*if (gameManager.gameOver)
        {
            return;
        }*/

        if(currentHealth > maxHealth) 
        {
        currentHealth = maxHealth;
        }

        if (!inDialogue) 
        {
            dialogueTimer = 0;
            timeSinceDialogue += Time.deltaTime;
        }
        else
        {
            dialogueTimer += Time.deltaTime;
        }
        if (inDialogue && Input.GetKey(KeyCode.E) && dialogueTimer >= dialogueCooldown)
        {
            dialogueManager.DisplayNextSentence();
            dialogueTimer = 0;
        }

        timeSinceDamage += Time.deltaTime;

        healthBar.fillAmount = currentHealth / 100f;

        if (inDialogue)
        {
            body.velocity = new Vector3(0, body.velocity.y, 0);
            return;
        }
        if (isDashing)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        else
        {
            // Only handle horizontal movement and flipping here
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (horizontalInput > 0.01f)
            {
                transform.localScale = new Vector3(correctScale.x, correctScale.y, correctScale.z);
                direction = 1;
            }
            else if (horizontalInput < -0.01f)
            {
                transform.localScale = new Vector3(correctScale.x * -1, correctScale.y, correctScale.z);
                direction = -1;
            }


            if(Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                canJump = false;
                isJumping = true;
            }

            if (Input.GetKey(KeyCode.Space) && isJumping)
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                if (jumpForceTime > 9f)
                {
                    isJumping = false;
                    body.velocity = new Vector2(body.velocity.x, 0f);
                }
                else
                {
                    jumpForceTime += .3f;
                }
            }

            if(isJumping && Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
                body.velocity = new Vector2(body.velocity.x, 0f);
            }

            if (isGrounded() && !Input.GetKey(KeyCode.Space))
            {
                canJump = true;
                jumpForceTime = 0f;
            }
            else
            {
                canJump = false;
            }

            if (Input.GetKeyDown(KeyCode.X) && isGrounded())
            {
                Jump();
            }
            if (Input.GetKeyDown(KeyCode.C) && isGrounded())
            {
                float trueJumpPower = jumpPower;
                jumpPower /= 1.3f;
                Jump();
                jumpPower = trueJumpPower;
            }
        }
        /*
                if (isDashing)
                {
                    return;
                }


                horizontalInput = Input.GetAxis("Horizontal");

                if (Input.GetKey(KeyCode.LeftShift) && canDash)
                {
                    StartCoroutine(Dash());
                }
                else
                {
                    body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

                    //Flip player when moving left/right
                    if (horizontalInput > 0.01f)
                    {
                        transform.localScale = Vector3.one;
                        direction = 1;
                    }

                    else if (horizontalInput < -0.01f)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        direction = -1;
                    }

                    if (Input.GetKey(KeyCode.Space) && isGrounded())
                    {
                        Jump();
                    }
                }
                //Walljump stuff
                if (wallJumpCooldown > 0.2f)
                {


                    body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

                    if (onWall() && isGrounded())
                    {
                        body.gravityScale = 0;
                        body.velocity = Vector2.zero;
                    }
                    else
                        body.gravityScale = 1;

                    if (Input.GetKey(KeyCode.Space))
                        Jump();
                }
                else
                    wallJumpCooldown += Time.deltaTime;
        */

        if (currentHealth <= 0)
        {
            Die();
        }

        if (body.velocity.y < maxFallSpeed * -1 && !isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, maxFallSpeed * -1);
        }
    }

    private void FixedUpdate()
    {
        /*
        if (isDashing)
        {
            return;
        }
        */
    }

    private void Jump()
    {
        if(isGrounded() && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S))
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
        else if(onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 2);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        audioSource.clip = dashSound;
        audioSource.Play();
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(polygonCollider.bounds.center, polygonCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return !onWall();
    }

    void Die()
    {
        gameManager.GameOver();
    }

    public void Respawn()
    {
        transform.position = currentSpawn;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Interactable")
        {
            interactAnimator.SetBool("CanInteract", true);
        }
        if (other.tag == "Talkable")
        {
            talkAnimator.SetBool("CanTalk", true);
        }
        if (other.tag == "Dialogue")
        {
            other.GetComponent<DialogueTrigger>().TriggerDialogue();
            if (other.gameObject != permanentDialogue)
            {
                Destroy(other.gameObject);
            }
        }
        if (other.tag == "Gauntlet")
        {
            other.GetComponent<GauntletTrigger>().StartGauntlet();
        }
        if(other.tag == "Hands")
        {
            if (other.GetComponent<FollowandSlam>().isSlamming)
            {
                currentHealth -= 10;
                UnityEngine.Debug.Log(currentHealth);
            }
        }
        if(other.tag == "Spawn")
        {
            currentSpawn = other.transform.position;
        }
        if (other.tag == "Pit")
        {
            currentHealth -= 5;
            transform.position = currentSpawn; 
        }
        if(other.tag == "Lava")
        {
            body.velocity = new Vector2(body.velocity.x, 15);
            currentHealth -= 10;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Interactable")
        {
            interactAnimator.SetBool("CanInteract", false);
        }
        if (other.tag == "Talkable")
        {
            talkAnimator.SetBool("CanTalk", false);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        UnityEngine.Debug.Log(other);
        if (other.gameObject.tag == "Pickup")
        {
            if(other.gameObject.GetComponent<PickupProperties>().pickupType == "Health")
            {
                currentHealth += other.gameObject.GetComponent<PickupProperties>().floatValue;
            }
            else if (other.gameObject.GetComponent<PickupProperties>().pickupType == "Damage")
            {
                GetComponent<PlayerAttacks>().damage += other.gameObject.GetComponent<PickupProperties>().floatValue;
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            if(timeSinceDamage > invincibleTime)
            {
                currentHealth -= 5;
            }
        }
        if (other.gameObject.tag == "Ground")
        {
            rb.gravityScale = 1f;
        }
        /*if (other.gameObject.tag == "Hands")
        {
            currentHealth -= 10;
            transform.position = new Vector3(transform.position.x + 3 * direction, transform.position.y + 5, transform.position.z);
        }*/
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Slope" && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.X)))
        {
            rb.gravityScale = 1f;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Moving")
        {
            transform.position = new Vector3(transform.position.x + other.gameObject.GetComponent<MovingPlatform>().distanceMoved /*(other.gameObject.GetComponent<MovingPlatform>().timeSinceSpawn * other.gameObject.GetComponent<MovingPlatform>().speed * transform.right.x)*/, transform.position.y, transform.position.z);
        }
        if(other.gameObject.tag == "Slope" && (!Input.GetKey(KeyCode.Space) || !Input.GetKey(KeyCode.X)))
        {
            rb.gravityScale = 2f;
        }
        /*else if (other.gameObject.tag == "Slope" && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.X)))
        {
            rb.gravityScale = 1f;
        }*/
    }
}
