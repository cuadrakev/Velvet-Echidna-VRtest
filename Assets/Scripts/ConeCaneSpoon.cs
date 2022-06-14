using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeCaneSpoon : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip fill;
    public AudioClip pour;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public GameObject coneCaneSpoon;
    public GameObject fullSpoon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CandyCane")
        {
            Destroy(other.gameObject);
            Instantiate(coneCaneSpoon);
            coneCaneSpoon.transform.position = transform.position;
            audioSource.PlayOneShot(pour);
            Destroy(this.gameObject);
        }

        if (tag =="Spoon" && other.tag == "Mixture")
        {
            Destroy(other.gameObject);
            Instantiate(fullSpoon);
            fullSpoon.transform.position = transform.position;
            audioSource.PlayOneShot(fill);
            Destroy(this.gameObject);
        }
    }
}
