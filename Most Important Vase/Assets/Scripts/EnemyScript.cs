using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    enum EnemyType { Grounded, Flying }

    //[SerializeField] private LayerMask groundLayer;
    //private BoxCollider2D collider;
    [SerializeField] private EnemyType enemyType;
    public float leftPatrolBound;
    public float rightPatrolBound;
    public float upPatrolBound;
    public float downPatrolBound;
    private bool goingLeft = false;
    private bool goingRight = false;
    private bool goingUp = false;
    private bool goingDown = false;
    private Vector3 startingPosition;

    public float movementSpeed;
    private Rigidbody2D rb;

    private float posScaleX;
    private float posScaleY;

    public float health = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //collider = GetComponent<BoxCollider2D>();

        if (enemyType == EnemyType.Grounded)
        {
            rb.gravityScale = 1f;
        }
        else if (enemyType == EnemyType.Flying)
        {
            rb.gravityScale = 0f;
        }

        startingPosition = transform.position;

        posScaleX = transform.localScale.x;
        posScaleY = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if ((enemyType == EnemyType.Grounded && (leftPatrolBound != 0 || rightPatrolBound != 0)) || (enemyType == EnemyType.Flying && (leftPatrolBound != 0 || rightPatrolBound != 0) && upPatrolBound == 0 && downPatrolBound == 0))
        {
            if (!goingLeft && !goingRight)
            {
                goingLeft = true;
            }
            else if (goingLeft && transform.position.x >= startingPosition.x - leftPatrolBound)
            {
                transform.localScale = new Vector3(posScaleX * -1, posScaleY, 1);
                rb.velocity = new Vector2(movementSpeed * Time.deltaTime * -1, 0f);
            }
            else if (goingRight && transform.position.x <= startingPosition.x + rightPatrolBound)
            {
                transform.localScale = new Vector3(posScaleX, posScaleY, 1);
                rb.velocity = new Vector2(movementSpeed * Time.deltaTime, 0f);
            }
            //failsafe
            else if (goingRight && goingLeft)
            {
                goingRight = false;
            }

            if (transform.position.x <= startingPosition.x - leftPatrolBound)
            {
                goingLeft = false;
                goingRight = true;
            }
            if (transform.position.x >= startingPosition.x + rightPatrolBound)
            {
                goingLeft = true;
                goingRight = false;
            }
        }
        //Note: Flying enemies will default to moving up/down
        //To make one move horizontally, set upPatrolBound and downPatrolBound to 0
        else if (enemyType == EnemyType.Flying && (upPatrolBound != 0 || downPatrolBound != 0)) 
        {
            if (!goingUp && !goingDown)
            {
                goingDown = true;
            }
            else if (goingDown && transform.position.y >= startingPosition.y - downPatrolBound)
            {
                rb.velocity = new Vector2(0f, movementSpeed * Time.deltaTime * -1);
            }
            else if (goingUp && transform.position.y <= startingPosition.y + upPatrolBound)
            {
                rb.velocity = new Vector2(0f, movementSpeed * Time.deltaTime);
            }
            //failsafe
            else if (goingUp && goingDown)
            {
                goingUp = false;
            }

            if (transform.position.y <= startingPosition.y - downPatrolBound)
            {
                goingDown = false;
                goingUp = true;
            }
            if (transform.position.y >= startingPosition.y + upPatrolBound)
            {
                goingDown = true;
                goingUp = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }
        else return;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Semisolid" && enemyType == EnemyType.Grounded)
        {
            rb.gravityScale = 1f;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Semisolid" && enemyType == EnemyType.Grounded)
        {
            rb.gravityScale = 0f;
        }
    }
}
