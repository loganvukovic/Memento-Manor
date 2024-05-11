using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private PolygonCollider2D polygonCollider;
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

    private void Awake()
    {
        //Grab references for rigidbody
        body = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if(isGrounded())
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


    private bool isGrounded()
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
}
