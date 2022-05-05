using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyRusty"))
        {
            transform.Rotate(0, 0, -90);
        }
    }
}
