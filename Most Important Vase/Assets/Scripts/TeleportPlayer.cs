using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    // Reference to the teleport destination
    public Transform teleportDestination;
    public SpriteRenderer spriteRenderer;
    public Sprite backgroundSprite;
    public BoxCollider2D invisWall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            // Teleport the player to the destination
            other.transform.position = teleportDestination.position;
            spriteRenderer.sprite = backgroundSprite;
            invisWall.enabled = true;
        }
    }
}
