using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject door;

    public AudioSource unlockSound;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            UnlockDoor();
            Destroy(this.gameObject);
        }
    }

    void UnlockDoor()
    {
        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<Animator>().SetBool("IsOpen", true);
        unlockSound.Play();
    }
}
