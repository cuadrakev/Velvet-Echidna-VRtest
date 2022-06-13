using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCreation : MonoBehaviour
{
    public GameObject bat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cookie")
        {
            Destroy(other.gameObject);
            Instantiate(bat);
            bat.transform.position = transform.position;
            Destroy(this.gameObject);
        }

    }
}
