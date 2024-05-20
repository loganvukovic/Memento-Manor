using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TeleportPlayer : MonoBehaviour
{

    public GameObject BlackOutSquare;
    public UnityEngine.UI.Image blackOutSquare;

    // Reference to the teleport destination
    public Transform teleportDestination;
    public SpriteRenderer spriteRenderer;
    public Sprite backgroundSprite;
    public BoxCollider2D invisWall;
    public GameObject player;

    void Start()
    {
        blackOutSquare.color = new Color(blackOutSquare.color.r, blackOutSquare.color.g, blackOutSquare.color.b, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            // Teleport the player to the destination
            StartCoroutine(FadeBlackOutSquare());
            //other.transform.position = teleportDestination.position;
            //spriteRenderer.sprite = backgroundSprite;
            invisWall.enabled = true;
        }
    }

    public IEnumerator FadeBlackOutSquare( int fadeSpeed = 1)
    {
        Color objectColor = BlackOutSquare.GetComponent<UnityEngine.UI.Image>().color;
        float fadeAmount;

        while (BlackOutSquare.GetComponent<UnityEngine.UI.Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            BlackOutSquare.GetComponent<UnityEngine.UI.Image>().color = objectColor;
            yield return null;
        }
        player.transform.position = teleportDestination.position;
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeFromBlack());
    }

    public IEnumerator FadeFromBlack(int fadeSpeed = 1)
    {
        Color objectColor = BlackOutSquare.GetComponent<UnityEngine.UI.Image>().color;
        float fadeAmount;

        while (BlackOutSquare.GetComponent<UnityEngine.UI.Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            BlackOutSquare.GetComponent<UnityEngine.UI.Image>().color = objectColor;
            yield return null;
        }
    }
    
}
