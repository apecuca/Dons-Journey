using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private float dampness = 20.0f;

    private void Update()
    {
        if (followTarget == null) 
            return;

        Vector3 newPos = Vector3.Lerp(transform.position, followTarget.position, dampness * Time.deltaTime);
        newPos.x = 0;
        newPos.z = -10.0f;
        transform.position = newPos;
    }
}
