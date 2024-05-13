using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    Transform cam; // Main Camera
    Vector3 camStartPos;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Keep the vertical position fixed and follow the camera horizontally
        transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);
    }
}
