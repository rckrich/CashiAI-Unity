using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    public Vector3 fixedPosition;

    void Start()
    {
        fixedPosition = transform.position; // Guarda la posici�n inicial
    }

    void LateUpdate()
    {
        transform.position = fixedPosition; // Reestablece la posici�n cada frame
    }
}
