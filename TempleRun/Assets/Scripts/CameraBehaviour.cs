using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will adjust camera to follow and face a target
/// </summary>
public class CameraBehaviour : MonoBehaviour
{
    [Tooltip("What the camera should be looking at")]
    public Transform target;

    [Tooltip("How offset will the camera be from the target")]
    public Vector3 offset = new Vector3(0, 3, -6);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if target is a valid object
        if (target != null)
        {
            // Set our position to an offset of target
            transform.position = target.position + offset;
            // Set our rotation to look at target
            transform.LookAt(target);
        }
    }
}
