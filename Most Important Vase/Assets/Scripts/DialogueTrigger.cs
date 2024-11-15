using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject player;
    public AudioClip newAudioClip;

    public void TriggerDialogue()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerMovement>().inDialogue = true;
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.newAudioClip = newAudioClip;
        dialogueManager.StartDialogue(dialogue);
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && Input.GetKey(KeyCode.E) && other.GetComponent<PlayerMovement>().inDialogue == false && other.GetComponent<PlayerMovement>().timeSinceDialogue >= 0.7f)
        {
            TriggerDialogue();
        }
    }
}
