using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeCaneSpoon : MonoBehaviour
{
    public GameObject coneCaneSpoon;
    public GameObject fullSpoon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CandyCane")
        {
            Destroy(other.gameObject);
            Instantiate(coneCaneSpoon);
            coneCaneSpoon.transform.position = transform.position + new Vector3(0,1,0);
            coneCaneSpoon.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
        }

        if (tag =="Spoon" && other.tag == "Mixture")
        {
            Destroy(other.gameObject);
            Instantiate(fullSpoon);
            fullSpoon.transform.position = transform.position + new Vector3(0, 1, 0); 
            fullSpoon.GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
        }
    }
}
