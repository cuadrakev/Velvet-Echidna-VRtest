using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.ColliderEvent;

public class ConeCaneSpoon : MonoBehaviour
{
    public GameObject coneCaneSpoon;
    public GameObject fullSpoon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CandyCane")
        {
            var newObject = Instantiate(coneCaneSpoon);
            newObject.transform.position = transform.position;
            Destroy(other.gameObject);
            GrabCombinedObject(gameObject, other.gameObject, newObject);
            newObject.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
        }

        if (tag =="Spoon" && other.tag == "Mixture")
        {
            if (!other.GetComponent<CouldronScript>().IsReady)
                return;
            var newObject = Instantiate(fullSpoon);
            newObject.transform.position = transform.position;
            GrabCombinedObject(gameObject, other.gameObject, newObject);
            newObject.GetComponent<AudioSource>().Play();
            Destroy(other.gameObject.transform.parent.transform.Find("Bubbles").gameObject);
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
        newObject.transform.rotation = grabber.grabberOrigin.rot;
        eventData.forceGrab = newObject;
    }
}
