using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; 

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player" && Input.GetKey(KeyCode.E) && other.GetComponent<PlayerMovement>().inDialogue == false && other.GetComponent<PlayerMovement>().timeSinceDialogue >= 0.7f)
        {
            TriggerDialogue();
            other.GetComponent<PlayerMovement>().inDialogue = true;
        }
    }
}
