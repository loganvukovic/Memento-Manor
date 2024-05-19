using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerScript;
    public GameObject gameOverScreen;
    public bool gameOver = false;
    public EnemyScript[] bosses;
    public BossScript[] bossScripts;
    public GameObject risingFloor;
    public GameObject fakeWizard;
    public BoxCollider2D gauntletTrigger;
    public BulletSpawner[] gauntletSpawners;
    public CircleCollider2D[] gauntletRanges;
    public bool paused = false;
    public GameObject pauseScreen;
    public float score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endingScoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !paused)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.P) && paused)
        {
            Unpause();
        }

        /*if(score < 0)
        {
            score = 0;
        }*/

        endingScoreText.text = "Score " + score;
    }

    public void GameOver()
    {
        if (Time.timeScale != 0 && !gameOver)
        {
            gameOver = true;
            scoreText.text = "Score: " + score;
            Time.timeScale = 0f;
            gameOverScreen.SetActive(true);
        }
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        playerScript.currentHealth = 100;
        score = 0;
        UnityEngine.Debug.Log(Time.timeScale);
        gameOverScreen.SetActive(false);
        gameOver = false;
        playerScript.Respawn();
        foreach (EnemyScript boss in bosses)
        {
            if (boss != null)
            {
                boss.health = boss.GetComponent<BossScript>().startingHealth;
            }
        }
        foreach (BossScript boss in bossScripts)
        {
            if (boss != null)
            {
                boss.currentPhase = 1;
                if(boss.activeBoss)
                {
                    boss.PhasePush();
                }
            }
        }
        risingFloor.transform.position = new Vector3(574.71f, -9.1f, -0.2674881f);
        risingFloor.GetComponent<RisingDeath>().timeSinceSpawn = 0;
        fakeWizard.transform.position = new Vector3(590.71f, 15.24f, -0.1353276f);
        fakeWizard.GetComponent<TeleportAndShoot>().StopAllCoroutines();
        fakeWizard.GetComponent<TeleportAndShoot>().isTeleporting = false;
        gauntletTrigger.enabled = true;

        foreach (CircleCollider2D collider in gauntletRanges)
        {
            collider.enabled = false;
        }
        foreach (BulletSpawner spawner in gauntletSpawners)
        {
            spawner.shotsLeft = spawner.totalShots;
        }
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }

    void Pause()
    {
        paused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    void Unpause()
    {
        paused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
}
