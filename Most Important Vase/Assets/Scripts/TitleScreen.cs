using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public bool showingControls = false;
    public GameObject startButton;
    public GameObject controlsButton;
    public GameObject controlsMenu;


    void Start()
    {
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        //UnityEngine.Debug.Log("yes");
    }

    public void Controls()
    {
        showingControls = true;
        startButton.SetActive(false);
        controlsButton.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void LeaveControls()
    {
        showingControls = false;
        startButton.SetActive(true);
        controlsButton.SetActive(true);
        controlsMenu.SetActive(false);
    }
}
