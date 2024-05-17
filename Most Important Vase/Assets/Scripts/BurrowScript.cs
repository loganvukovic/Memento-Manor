using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowScript : MonoBehaviour
{
    public Transform playerTransform;
    public float followSpeed = 5f;
    public float raiseDelay = 2f;
    public float raiseSpeed = 10f;
    public float fallSpeed = 20f;
    public float resumeDelay = 0.5f;

    public bool isRaising = false;

    public float burrowCooldown = 7f;
    public float timeSinceBurrow;
    public GameObject player;

    public BossScript bossScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerMovement>().inDialogue && (bossScript.currentPhase == 1 || bossScript.currentPhase == 2 || bossScript.currentPhase == 3 || bossScript.currentPhase == 4))
        {
            if (!isRaising && timeSinceBurrow >= burrowCooldown)
            {
                FollowPlayer();
            }
            if (!isRaising && Mathf.Abs(transform.position.x - playerTransform.position.x) < 0.1f)
            {
                StartCoroutine(Burrow());
            }
        }
        timeSinceBurrow += Time.deltaTime;
    }

    private void FollowPlayer()
    {
        GetComponent<Animator>().SetBool("Burrowed", true);
        float newX = Mathf.MoveTowards(transform.position.x, playerTransform.position.x, followSpeed * Time.deltaTime);
        transform.position = new Vector2(newX, transform.position.y);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private IEnumerator Burrow()
    {
        isRaising = true;
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y + 9);
        Vector2 landPosition = new Vector2(playerTransform.position.x, transform.position.y);
        yield return new WaitForSeconds(raiseDelay);

        GetComponent<BurrowBullets>().ShootBulletsUp();
        GetComponent<Animator>().SetBool("Burrowed", false);

        // Move quickly towards the player
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, raiseSpeed * Time.deltaTime);
            yield return null;
        }

        GetComponent<BurrowBullets>().ShootBulletsDown();

        yield return new WaitForSeconds(resumeDelay);

        // Raise back to the original position
        while ((Vector2)transform.position != landPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, landPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        isRaising = false;
        GetComponent<BoxCollider2D>().enabled = true;
        timeSinceBurrow = 0;
    }
}
