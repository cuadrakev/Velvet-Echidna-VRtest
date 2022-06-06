using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouldronScript : MonoBehaviour
{
    public Color startingColor;
    public Color endColor;
    private List<string> insertedIngredients = new List<string>();
    int maxIngredients = 2;
    private Material mixtureMaterial;
    private Material bubblesMaterial;

    void Start()
    {
        mixtureMaterial = GetComponent<Renderer>().material;
        bubblesMaterial = transform.parent.Find("Bubbles").gameObject.GetComponent<ParticleSystemRenderer>().material;
        UpdateMixture();
    }

    void UpdateMixture()
    {
        int ingredientCount = insertedIngredients.Count;
        Color currentColor = Color.Lerp(startingColor, endColor, ((float)ingredientCount / (float)maxIngredients));
        mixtureMaterial.SetColor("_Color", currentColor);
        bubblesMaterial.SetColor("_Color", currentColor);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ingredient"))
        {
            insertedIngredients.Add(collider.gameObject.name);
            Destroy(collider.gameObject);
            UpdateMixture();
        }
    }
}
