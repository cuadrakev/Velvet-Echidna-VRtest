using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.ColliderEvent;

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
            
            var newObject = Instantiate(combinedObject);
            newObject.transform.position = transform.position;
            GrabCombinedObject(gameObject, other.gameObject, newObject);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void GrabCombinedObject(GameObject obj1, GameObject obj2, GameObject newObject)
    {
        ColliderButtonEventData eventData;
        BasicGrabbable.IGrabber grabber;
        if (obj1.GetComponent<BasicGrabbable>().grabbedEvent != null)
        {
            eventData = obj1.GetComponent<BasicGrabbable>().grabbedEvent;
            grabber = obj1.GetComponent<BasicGrabbable>().currentGrabber;
        }
        else if(obj2.GetComponent<BasicGrabbable>().grabbedEvent != null)
        {
            eventData = obj2.GetComponent<BasicGrabbable>().grabbedEvent;
            grabber = obj2.GetComponent<BasicGrabbable>().currentGrabber;
        }
        else
        {
            return;
        }

        newObject.transform.position = grabber.grabberOrigin.pos;
        eventData.forceGrab = newObject;
    }
}
