using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralSpawners : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private int numberOfCircles = 1000;
    [SerializeField] private float spiralSpacing = 0.2f;
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float expansionSpeed = 0.5f;
    [SerializeField] private float startDelay = 2f; 

    private GameObject[] circles;
    private float[] angles;
    private float[] distances;
    private bool isActive = false;
    private float baseSpacing;
    private float startTime;
    private float openDelay = 45f;

    private void Start()
    {
        circles = new GameObject[numberOfCircles];
        angles = new float[numberOfCircles];
        distances = new float[numberOfCircles];
        baseSpacing = spiralSpacing;

        StartCoroutine(StartAfterDelay());
    }

    private IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        isActive = true;
        SpawnCircles();

        startTime = Time.time;
    }

    private void SpawnCircles()
    {
        float goldenAngle = 137.5f * Mathf.Deg2Rad;

        for (int i = 0; i < numberOfCircles; i++)
        {
            angles[i] = i * goldenAngle;
            distances[i] = Mathf.Sqrt(i) * spiralSpacing;

            Vector3 position = new Vector3(
                Mathf.Cos(angles[i]) * distances[i],
                Mathf.Sin(angles[i]) * distances[i],
                0
            );

            circles[i] = Instantiate(circlePrefab, position, Quaternion.identity);
            circles[i].transform.parent = transform;

            Renderer renderer = circles[i].GetComponent<Renderer>();
            float shadeFactor = 0.7f;
            Color color;
            if (i % 3 == 0)
                color = Color.red;
            else if (i % 3 == 1)
                color = Color.white;
            else
               color = Color.blue;
            Color shadedColor = new Color(color.r * shadeFactor, color.g * shadeFactor, color.b * shadeFactor);
            renderer.material.color = shadedColor; 
        }
        
    }

    private void Update()
    {
        if (!isActive) return;

        for (int i = 0; i < numberOfCircles; i++)
        {
            if (Time.time - startTime >= openDelay && Time.time - startTime <= openDelay + 25)
            {
                // Gradually expand the distance after a certain time (openDelay)
                distances[i] = Mathf.Sqrt(i) * spiralSpacing + (Time.time - startTime - openDelay) * expansionSpeed;
            }
            else
            {
                // Keep the distance close to the center initially
                distances[i] = Mathf.Sqrt(i) * spiralSpacing;
            }

            angles[i] += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;

            Vector3 newPosition = new Vector3(
                Mathf.Cos(angles[i]) * distances[i],
                Mathf.Sin(angles[i]) * distances[i],
                distances[i] * -1.5f
            );

            circles[i].transform.position = newPosition;
        }
    }
}

