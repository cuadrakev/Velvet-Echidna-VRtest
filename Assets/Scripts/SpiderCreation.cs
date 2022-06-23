using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.ColliderEvent;

public class SpiderCreation : MonoBehaviour
{
    public GameObject spider2;
    public GameObject spiderFull;
    private void OnTriggerEnter(Collider other)
    {
        if (tag=="SpiderCandyYellow" && other.tag == "SpiderCandyRed")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            var newObject = Instantiate(spider2);
            newObject.transform.position = transform.position;
            GrabCombinedObject(gameObject, other.gameObject, newObject);
            newObject.GetComponent<AudioSource>().Play();
        }

        if (tag == "Spider2" && other.tag == "JellyBean")
        {
            var newObject = Instantiate(spiderFull);
            newObject.transform.position = transform.position;
            GrabCombinedObject(gameObject, other.gameObject, newObject);
            newObject.GetComponent<AudioSource>().Play();
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
