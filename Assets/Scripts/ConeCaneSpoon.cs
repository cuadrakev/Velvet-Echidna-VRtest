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
            coneCaneSpoon.transform.position = transform.position;
            Destroy(this.gameObject);
        }

        if (tag =="Spoon" && other.tag == "Mixture")
        {
            Destroy(other.gameObject);
            Instantiate(fullSpoon);
            fullSpoon.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
}
