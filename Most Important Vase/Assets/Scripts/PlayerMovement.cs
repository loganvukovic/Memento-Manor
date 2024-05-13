using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
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
    }

    // Update is called once per frame
    void Update()
    {
        //camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);

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

        if (inDialogue)
        {
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
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    public bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(polygonCollider.bounds.center, polygonCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
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
        UnityEngine.Debug.Log("Am ded");
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
    }
}
