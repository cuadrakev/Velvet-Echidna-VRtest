using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCane : MonoBehaviour
{
    public GameObject combinedObject;

    private Vector3 _startingPos;
    private Rigidbody _rigidbody;

    void Start()
    {
        _startingPos = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!_rigidbody.useGravity && transform.position!=_startingPos)
        {
            _rigidbody.useGravity = true;
        }
    }


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
