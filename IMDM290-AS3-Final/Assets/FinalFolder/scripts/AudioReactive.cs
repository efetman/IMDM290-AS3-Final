// UMD IMDM290 
// Instructor: Myungin Lee
// modified by Olivia DiJulio (and Elana Fetman)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactives : MonoBehaviour
{
    GameObject[] spheres;
    static int numSpherePerRow = 20;
    static int numRows = 5;
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction;
    float transitionProgress = 0f;

    [SerializeField] private float startDelay = 2f; // Delay before spawning starts
    [SerializeField] private float sphereSpawnDelay = 0.05f; // Delay between sphere spawns
    public float transitionSpeed = 1f; // Controls how fast the shape forms

    private Vector3[] startPositions; // Initial line positions
    private Vector3[] targetPositions; // Target positions for "a"
    
    private int totalSpheres;

    private void Start()
    {
        totalSpheres = numSpherePerRow * numRows;
        spheres = new GameObject[totalSpheres];
        initPos = new Vector3[totalSpheres];
        startPosition = new Vector3[totalSpheres];
        endPosition = new Vector3[totalSpheres];
        startPositions = new Vector3[totalSpheres];
        targetPositions = new Vector3[totalSpheres];

        float spacingX = 3f;
        float spacingZ = 3f;

        for (int col = 0; col < numSpherePerRow; col++) 
        {
            for (int row = 0; row < numRows; row++) 
            {
                int index = row * numSpherePerRow + col;
                float posX = (col - (numSpherePerRow - 1) / 2f) * spacingX;
                float posZ = ((row - (numRows - 1) / 2f) * spacingZ) - 25f;
                float posY = (row * spacingX) - 5f;

                startPosition[index] = new Vector3(posX, posY, posZ);
                endPosition[index] = new Vector3(posX, posY, posZ);
                startPositions[index] = startPosition[index];
                targetPositions[index] = GetAPosition(index);
            }
        }

        StartCoroutine(SpawnSpheresOneByOne());
    }

    private IEnumerator SpawnSpheresOneByOne() //loops through collection 1 by 1, allowing for delay, building, and then deay before the next interation
    {
        yield return new WaitForSeconds(startDelay); // Initial delay before spawning

        for (int i = 0; i < spheres.Length; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];
            spheres[i].SetActive(false); // Start hidden

            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            Color color = (i % 3 == 0) ? Color.red : (i % 3 == 1) ? Color.white : Color.blue;
            sphereRenderer.material.color = color;

            yield return new WaitForSeconds(sphereSpawnDelay); // Delay before next sphere appears
            spheres[i].SetActive(true);
        }
    }

    private void Update()
    {
        time += Time.deltaTime * AudioSpectrum.audioAmp;

        if (Time.time >= 67 && Time.time <= 74)
        {
            // Transition to the "a" shape
            transitionProgress += Time.deltaTime * transitionSpeed;
            transitionProgress = Mathf.Clamp01(transitionProgress); // Keep between 0 and 1

            for (int i = 0; i < totalSpheres; i++)
            {
                spheres[i].transform.position = Vector3.Lerp(startPositions[i], targetPositions[i], transitionProgress);
            }
        }
        else
        {
            // Oscillate spheres based on audio input
            for (int i = 0; i < spheres.Length; i++)
            {
                if (spheres[i] == null || !spheres[i].activeSelf) continue;

                lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;
                spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
                float verticalOscillation = Mathf.Sin(time * (i + 1) * 0.5f) * AudioSpectrum.audioAmp;

                Vector3 currentPosition = spheres[i].transform.position;
                currentPosition.y += verticalOscillation;
                spheres[i].transform.position = currentPosition;

                float scale = 1f + AudioSpectrum.audioAmp / 2;
                spheres[i].transform.localScale = new Vector3(scale, scale, scale);
                spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1.0f, 1.0f);
            }
        }
    }

    // Method to calculate the position of the spheres in the exact "a" shape
    Vector3 GetAPosition(int i)
    {
        float r = 1f; // Radius for the loop
        float t = (float)i / totalSpheres * 2 * Mathf.PI; // Spread spheres evenly

        if (i < totalSpheres * 0.6f)
        {
            // Horizontal line from -r to r
            float x = Mathf.Lerp(-r, r, (float)i / (totalSpheres * 0.6f));  
            float y = 0f; // y = 0 for horizontal line
            return new Vector3(x, y, -10f);
        }
        else
        {
            float progress = (i - totalSpheres * 0.6f) / (totalSpheres * 0.4f);

            // Diagonal lines meeting at (0, 3)
            float yMeet = 3f;

            Vector3 endPoint1 = new Vector3(-2f, -2f, -10f); // Left diagonal endpoint
            Vector3 endPoint2 = new Vector3(2f, -2f, -10f);  // Right diagonal endpoint

            if (progress < 0.5f)
            {
                // First diagonal (top-left to (0, 3))
                float x = Mathf.Lerp(endPoint1.x, 0f, progress * 2f);
                float y = Mathf.Lerp(endPoint1.y, yMeet, progress * 2f);
                return new Vector3(x, y, -10f);
            }
            else
            {
                // Second diagonal (top-right to (0, 3))
                float x = Mathf.Lerp(0f, endPoint2.x, (progress - 0.5f) * 2f);
                float y = Mathf.Lerp(yMeet, endPoint2.y, (progress - 0.5f) * 2f);
                return new Vector3(x, y, -10f);
            }
        }
    }
}
