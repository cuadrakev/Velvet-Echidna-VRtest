using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCane : MonoBehaviour
{
    public GameObject combinedObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Candy"))
        {
            Destroy(other.gameObject);
            Instantiate(combinedObject);
            combinedObject.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
}
