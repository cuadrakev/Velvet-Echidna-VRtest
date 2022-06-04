using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderCreation : MonoBehaviour
{
    public GameObject spider2;
    public GameObject spiderFull;
    private void OnTriggerEnter(Collider other)
    {
        if (tag=="SpiderCandyYellow" && other.tag == "SpiderCandyRed")
        {
            Destroy(other.gameObject);
            Instantiate(spider2);
            spider2.transform.position = transform.position;
            Destroy(this.gameObject);
        }

        if (tag == "Spider2" && other.tag == "JellyBean")
        {
            Destroy(other.gameObject);
            Instantiate(spiderFull);
            spiderFull.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
}
