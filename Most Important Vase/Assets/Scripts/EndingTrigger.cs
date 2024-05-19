using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    public GameObject endingScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        endingScreen.SetActive(true);
        //Time.timeScale = 0f;
    }
}
