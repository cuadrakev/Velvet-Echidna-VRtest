using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageDoor : MonoBehaviour
{
    public GameObject floor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyRusty"))
        {
            transform.Rotate(0, 0, -75);
            floor.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
    }
}
