// UMD IMDM290 
// Instructor: Myungin Lee
// sphere movement by Olivia DiJulio 
// spawning and "a" by Elana Fetman

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactive : MonoBehaviour
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
    public float transitionSpeed = 1f; 
    private Vector3[] startPositions; // Initial line positions
    private Vector3[] targetPositions; // Target positions for "a"
    private int totalSpheres;
    private float timer = 0f;

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
    private IEnumerator SpawnSpheresOneByOne()
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

        // 1st part 
        if (Time.time <= 31f)
        {
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
        // 2nd part 
        else if (Time.time > 31f && Time.time <= 62f)
        {
            for (int i = 0; i < totalSpheres; i++)
            {
                if (spheres[i] == null || !spheres[i].activeSelf) continue;

                float waveSpeed = 2f;
                float waveStrength = 5f;
                float waveValue = Mathf.Sin(time * waveSpeed + (i * 0.1f)) * waveStrength; 
                Vector3 newPos = startPosition[i] + new Vector3(waveValue, 0f, 0f); 

                spheres[i].transform.position = newPos;

                float scale = 1f + AudioSpectrum.audioAmp;
                spheres[i].transform.localScale = new Vector3(scale, scale, scale);

                spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1.0f, 1.0f);
            }
        }
        // 3rd part 
        else if (Time.time > 62f && Time.time <= 67f)
        {
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
        // 4th part 
        else if (Time.time >= 67f && Time.time <= 72.6f)
        {
            transitionProgress += Time.deltaTime * transitionSpeed;
            transitionProgress = Mathf.Clamp01(transitionProgress); // Keep it between 0 and 1

            for (int i = 0; i < totalSpheres; i++)
            {
                // Transition spheres to new target positions, making them form the "a" shape
                spheres[i].transform.position = Vector3.Lerp(startPositions[i], targetPositions[i], transitionProgress);
            }
        }
        // 5th part 
        else if (Time.time > 72.6f && Time.time <= 84)
        {
            for (int i = 0; i < totalSpheres; i++)
            {
                if (spheres[i] == null || !spheres[i].activeSelf) continue;

                float waveSpeed = 4f;
                float waveStrength = 7f;
                float waveValue = Mathf.Sin(time * waveSpeed + (i * 0.1f)) * waveStrength; 
                Vector3 newPos = startPosition[i] + new Vector3(waveValue, 0f, 0f); 

                spheres[i].transform.position = newPos;

                float scale = 1f + AudioSpectrum.audioAmp/2f;
                spheres[i].transform.localScale = new Vector3(scale, scale, scale);

                spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1.0f, 1.0f);
            }
        }
        else if(Time.time > 85){
            for (int i = 0; i < totalSpheres; i++)
            {
                spheres[i].SetActive(false);
            }
        }
        
    }



    // Method to calculate the position of the spheres in the exact "a" shape
    Vector3 GetAPosition(int i)
    {
        float r = 3f; // Radius for the loop
        float t = (float)i / totalSpheres * 2 * Mathf.PI; // Spread spheres evenly

        if (i < totalSpheres * 0.6f)
        {
            // Horizontal line from -r to r
            float x = Mathf.Lerp(-r, r , (float)i / (totalSpheres * 0.6f));  
            float y = -3f; // y = 0 for horizontal line
            return new Vector3(x, y, -10f);
        }
        else
        {
            float progress = (i - totalSpheres * 0.6f) / (totalSpheres * 0.4f);

            // Diagonal lines meeting at (0, 3)
            float yMeet = 3f;

            Vector3 endPoint1 = new Vector3(-6f, -6f, -10f); // Left diagonal endpoint
            Vector3 endPoint2 = new Vector3(6f, -6f, -10f);  // Right diagonal endpoint

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



