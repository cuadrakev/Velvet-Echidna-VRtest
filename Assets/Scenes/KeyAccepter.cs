using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAccepter : MonoBehaviour
{
    public string KeyTag;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag(KeyTag))
        {
            Destroy(gameObject);
            Destroy(collider.gameObject);
        }
    }
}