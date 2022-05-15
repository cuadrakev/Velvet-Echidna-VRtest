using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouldronScript : MonoBehaviour
{
    private List<string> insertedIngredients = new List<string>();

    void Update()
    {
        if(insertedIngredients.Count == 2)
        {
            //if(insertedIngredients[0] == "Ingredient1" && insertedIngredients[0] == "Ingredient2")
            {
                GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                transform.parent.Find("Bubbles").gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color", Color.white);
                insertedIngredients.Clear();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
      if(collider.gameObject.CompareTag("Ingredient"))
      {
          insertedIngredients.Add(collider.gameObject.name);
          Destroy(collider.gameObject);
      }
    }
}
