// UMD IMDM290 
// Instructor: Myungin Lee
// modified by Olivia DiJulio

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
    float lerpFraction; // Lerp point between 0~1
    float t;
    

    // Start is called before the first frame update
    void Start()
    {
        // rows 
        int totalSpheres = numRows * numSpherePerRow;
        
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[totalSpheres];
        initPos = new Vector3[totalSpheres]; // Start positions
        startPosition = new Vector3[totalSpheres]; 
        endPosition = new Vector3[totalSpheres]; 

        // rows loop 

        float spacingX = 3f; 
        float spacingZ = 3f; 

         for (int col = 0; col < numSpherePerRow; col++) // horizontal 
        {
            for (int row = 0; row < numRows; row++) //vertical 
            {
                int index = row * numSpherePerRow + col;
                float posX = (col - (numSpherePerRow - 1) / 2f) * spacingX; 
                float posZ = (row - (numRows - 1) / 2f) * spacingZ; 
                float posY = row * spacingX; 
                
                startPosition[index] = new Vector3(posX, posY, posZ);
                endPosition[index] = new Vector3(posX, posY, posZ);
            }
        } 

           for (int i = 0; i < totalSpheres; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];

            
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            Color color;
            switch (i % 3)
            {
                case 0:
                    color = Color.red; 
                    break;
                case 1:
                    color = Color.white; 
                    break;
                case 2:
                    color = Color.blue; 
                    break;
                default:
                    color = Color.white;
                    break;
            }
            sphereRenderer.material.color = color;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        // ***Here, we use audio Amplitude, where else do you want to use?
        // Measure Time 
        // Time.deltaTime = The interval in seconds from the last frame to the current one
        // but what if time flows according to the music's amplitude?

        time += Time.deltaTime * AudioSpectrum.audioAmp; 

        for (int i =0; i < spheres.Length; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;
            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction); 
            float verticalOscillation = Mathf.Sin(time * (i + 1) * 0.5f) * AudioSpectrum.audioAmp; 

            // y oscillation
            Vector3 currentPosition = spheres[i].transform.position;
            currentPosition.y += verticalOscillation; 
            spheres[i].transform.position = currentPosition;
                       
            float scale = 1f + AudioSpectrum.audioAmp/2; // adjust object reactivity here // set lower for vocals only 
            spheres[i].transform.localScale = new Vector3(scale, scale, scale);
            spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1.0f, 1.0f);
        }
    }
}