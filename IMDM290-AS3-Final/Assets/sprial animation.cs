using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralSpawner : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private int numberOfCircles = 1000;
    [SerializeField] private float spiralSpacing = 0.2f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float expansionSpeed = 0.5f;

    private GameObject[] circles;
    private float[] angles;
    private float[] distances;
    private float currentRotation = 0f;
    private float baseSpacing;
    private Material redMaterial;
    private Material whiteMaterial;
    private Material blueMaterial;

    private void Start()
    {
        // Create materials
        redMaterial = new Material(Shader.Find("Standard"));
        redMaterial.color = Color.red;
        whiteMaterial = new Material(Shader.Find("Standard"));
        whiteMaterial.color = Color.white;
        blueMaterial = new Material(Shader.Find("Standard"));
        blueMaterial.color = Color.blue;

        circles = new GameObject[numberOfCircles];
        angles = new float[numberOfCircles];
        distances = new float[numberOfCircles];
        baseSpacing = spiralSpacing;
        SpawnCircles();
    }

    private void SpawnCircles()
    {
        float goldenAngle = 137.5f * Mathf.Deg2Rad;
        for (int i = 0; i < numberOfCircles; i++)
        {
            angles[i] = i * goldenAngle;
            distances[i] = Mathf.Sqrt(i) * baseSpacing;
            Vector3 position = new Vector3(
                Mathf.Cos(angles[i]) * distances[i],
                Mathf.Sin(angles[i]) * distances[i],
                0
            );

            circles[i] = Instantiate(circlePrefab, position, Quaternion.identity);
            circles[i].transform.parent = transform;

            // Assign colors in a repeating pattern
            Renderer renderer = circles[i].GetComponent<Renderer>();
            if (i % 3 == 0)
                renderer.material = redMaterial;
            else if (i % 3 == 1)
                renderer.material = whiteMaterial;
            else
                renderer.material = blueMaterial;
        }
    }

    private void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;

        for (int i = 0; i < numberOfCircles; i++)
        {
            angles[i] += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
            distances[i] += expansionSpeed * Time.deltaTime;

            Vector3 newPosition = new Vector3(
                Mathf.Cos(angles[i]) * distances[i],
                Mathf.Sin(angles[i]) * distances[i],
                distances[i] * -1.5f
            );

            circles[i].transform.position = newPosition;

            if (distances[i] > baseSpacing * Mathf.Sqrt(numberOfCircles) * 2)
            {
                distances[i] = 0;
            }
        }
    }
}