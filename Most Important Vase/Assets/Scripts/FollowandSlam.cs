using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class FollowandSlam : MonoBehaviour
{
    public Transform playerTransform;
    public float followSpeed = 5f;
    public float slamDelay = 2f;
    public float slamSpeed = 10f;
    public float fallSpeed = 20f;
    public float resumeDelay = 0.5f;
    public float raiseSpeed = 10f;

    public bool isSlamming = false;
    private Vector2 originalPosition;

    public FollowandSlam otherHandScript;
    public bool myTurn;

    public BossScript bossScript;
    public GameObject player;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (myTurn && (bossScript.currentPhase == 1 || bossScript.currentPhase == 4) && !player.GetComponent<PlayerMovement>().inDialogue)
        {
            if (!isSlamming)
            {
                FollowPlayer();
            }
            if (!isSlamming && Mathf.Abs(transform.position.x - playerTransform.position.x) < 0.1f)
            {
                StartCoroutine(SlamAfterDelay());
            }
        }
        else if (!isSlamming)
        {
            StartCoroutine(ReturnToStart());

        }
    }

    private void FollowPlayer()
    {
        float newX = Mathf.MoveTowards(transform.position.x, playerTransform.position.x, followSpeed * Time.deltaTime);
        transform.position = new Vector2(newX, transform.position.y);
    }

    private IEnumerator SlamAfterDelay()
    {
        isSlamming = true;
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y - 9);
        yield return new WaitForSeconds(slamDelay);

        // Move quickly towards the player
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        GetComponent<HandSlamBullets>().ShootBullets();

        yield return new WaitForSeconds(resumeDelay);

        // Raise back to the original position
        while ((Vector2)transform.position != originalPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, raiseSpeed * Time.deltaTime);
            yield return null;
        }

        isSlamming = false;
        myTurn = false;
        otherHandScript.myTurn = true;
    }

    private IEnumerator ReturnToStart()
    {
        while ((Vector2)transform.position != originalPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPosition, raiseSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
