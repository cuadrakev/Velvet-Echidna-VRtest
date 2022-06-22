using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoor : MonoBehaviour
{
    public GameObject floor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyRusty"))
        {
            transform.Rotate(-60, 0, 0);
            floor.SetActive(true);
        }
    }
}