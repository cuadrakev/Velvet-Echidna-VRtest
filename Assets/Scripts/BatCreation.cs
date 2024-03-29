using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.ColliderEvent;

public class BatCreation : MonoBehaviour
{
    public GameObject bat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cookie")
        {
            var newObject = Instantiate(bat);
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
        else if (obj2.GetComponent<BasicGrabbable>().grabbedEvent != null)
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
