using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerScript;
    public GameObject gameOverScreen;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        //gameOver = true;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        UnityEngine.Debug.Log(Time.timeScale);
    }
}
