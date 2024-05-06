/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public GameObject slimePrefab;
    public float duplicationRadius = 0.25f;
    public int numberOfDuplicates = 2;
    public float sizeReductionFactor = 2f;
    public float minScaleThreshold = 0.5f;

    private Light pointLight;

    void Start()
    {
        pointLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (transform.localScale.x >= minScaleThreshold) // Check scale threshold
                    {
                        DuplicateSlime();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    void DuplicateSlime()
    {
        float angleIncrement = 360f / numberOfDuplicates;
        float initialAngleOffset = Random.Range(0f, 360f); // Random offset for initial angle

        for (int i = 0; i < numberOfDuplicates; i++)
        {
            float angle = (i * angleIncrement + initialAngleOffset) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * duplicationRadius, 0f, Mathf.Sin(angle) * duplicationRadius);
            GameObject duplicate = Instantiate(slimePrefab, transform.position + offset, Quaternion.identity);
            duplicate.transform.localScale /= sizeReductionFactor;

            // Adjust light intensity
            Light duplicateLight = duplicate.GetComponentInChildren<Light>();
            if (duplicateLight != null)
            {
                duplicateLight.intensity = pointLight.intensity / 2f;
            }
        }
        Destroy(gameObject);
    }
}

*/