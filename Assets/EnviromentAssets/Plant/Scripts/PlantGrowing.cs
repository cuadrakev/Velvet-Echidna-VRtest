using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowing : MonoBehaviour
{
    public List<MeshRenderer> plantMeshes;
    public float timeToGrow;
    public float refreshRate;
    [Range(0, 5)]
    public float minGrow;
    [Range(0, 5)]
    public float maxGrow;

    private List<Material> plantMaterials = new List<Material>();
    private bool fullyGrown;

    public GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < plantMeshes.Count; i++)
        {
            for (int j = 0; j < plantMeshes[i].materials.Length; j++)
            {
                if (plantMeshes[i].materials[j].HasProperty("_AnimationSpeed"))
                {
                    plantMeshes[i].materials[j].SetFloat("_AnimationSpeed", minGrow);
                    plantMaterials.Add(plantMeshes[i].materials[j]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GrowPlant(Material mat)
    {
        float growValue = mat.GetFloat("_AnimationSpeed");

        if (!fullyGrown)
        {
            while (growValue < maxGrow)
            {
                growValue += 1 / (timeToGrow / refreshRate);
                mat.SetFloat("_AnimationSpeed", growValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            growValue = maxGrow;
            //mat.SetFloat("_AnimationSpeed", growValue);

        }

        if (growValue >= maxGrow)
        {
            fullyGrown = true;
        }
        else
        {
            fullyGrown = false;
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Spoon"))
        {
            for (int i = 0; i < plantMaterials.Count; i++)
            {
                StartCoroutine(GrowPlant(plantMaterials[i]));
            }

            Instantiate(key);
            key.transform.position = transform.position + new Vector3(1, 0, 0);
            Destroy(this.gameObject);

        }

    }
}
