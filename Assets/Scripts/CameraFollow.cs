using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Set this in the Inspector to be the empty GameObject
    public float yOffset = -5f;

    void LateUpdate()
    {
        if (target != null)
        {
            // Only adjust the position, not the rotation
            float targetY = target.position.y + yOffset;
            transform.position = new Vector3(target.position.x, targetY, transform.position.z);
        }
    }
}
