using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyFall : MonoBehaviour
{
    public GameObject key;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plant"))
        {
            //Destroy(other.gameObject);
            Instantiate(key);
            key.transform.position = transform.position + new Vector3(-2, 0, 0);
            Destroy(this.gameObject);
        }
    }
}
