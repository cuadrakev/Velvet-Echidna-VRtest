using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouldronScript : MonoBehaviour
{
    public Color startingColor;
    public Color endColor;
    public int maxIngredients = 2;
    private List<string> insertedIngredients = new List<string>();
    private Material mixtureMaterial;
    private Material bubblesMaterial;
    
    private bool mixtureReady;
    public bool IsReady
    {
        get => mixtureReady;
    }

    void Start()
    {
        mixtureMaterial = GetComponent<Renderer>().material;
        bubblesMaterial = transform.parent.Find("Bubbles").gameObject.GetComponent<ParticleSystemRenderer>().material;
        mixtureReady = false;
        UpdateMixture();
    }

    void UpdateMixture()
    {
        int ingredientCount = insertedIngredients.Count;
        Color currentColor = Color.Lerp(startingColor, endColor, ((float)ingredientCount / (float)maxIngredients));
        mixtureMaterial.SetColor("_Color", currentColor);
        bubblesMaterial.SetColor("_Color", currentColor);

        if (insertedIngredients.Count == 2)
            mixtureReady = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Contains("Spider3") || collider.gameObject.name.Contains("Bat"))
        {
            insertedIngredients.Add(collider.gameObject.name);
            Destroy(collider.gameObject);
            UpdateMixture();
        }
    }
}
