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
    public bool goingLeft = false;
    public bool goingRight = false;
    public Vector3 startingPosition;

    public float movementSpeed;
    private Rigidbody2D rb;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyType == EnemyType.Grounded && (leftPatrolBound != 0 || rightPatrolBound != 0))
        {
            if (!goingLeft && !goingRight)
            {
                goingLeft = true;
            }
            else if (goingLeft && transform.position.x >= startingPosition.x - leftPatrolBound)
            {
                rb.velocity = new Vector2(movementSpeed * Time.deltaTime * -1, 0f);
            }
            else if (goingRight && transform.position.x <= startingPosition.x + rightPatrolBound)
            {
                rb.velocity = new Vector2(movementSpeed * Time.deltaTime, 0f);
            }
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
    }
}
