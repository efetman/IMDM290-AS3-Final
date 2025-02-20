// UMD IMDM290 
// Instructor: Myungin Lee
// All the same Lerp but using audio

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 10; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;
    

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            //float r = 1f;
            //startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        
            //startPosition[i] = new Vector3(r * Random.Range(-0.5f, 0.5f), r * Random.Range(-0.5f, 0.5f), r * Random.Range(-0.5f, 0.5f));
       
            //r = 3f; // radius of the circle
            // Circular end position
            //endPosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere));

            // end in a line 
            float spacing = 2f; // Space between spheres
            startPosition[i] = new Vector3((i - (numSphere - 1) / 2f) * spacing, 0.5f, -1f);
            endPosition[i] = new Vector3((i - (numSphere - 1) / 2f) * spacing, 0.5f, -1f);
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

            // Position
            initPos[i] = startPosition[i];
            spheres[i].transform.position = initPos[i];

            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            // float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            // Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            // sphereRenderer.material.color = color;

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

        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;
             spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction); 

            // Lerp logic. Update position       
            // t = i* 2 * Mathf.PI / numSphere;
            // spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            // float scale = 1f + AudioSpectrum.audioAmp;
            // spheres[i].transform.localScale = new Vector3(scale, 1f, 1f);
            // spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 2f, 2f);
            
            float verticalOscillation = Mathf.Sin(time * (i + 1) * 0.5f) * AudioSpectrum.audioAmp; // Customize frequency for each sphere

            // Update the Y position to create up and down oscillation
            Vector3 currentPosition = spheres[i].transform.position;
            currentPosition.y += verticalOscillation; // Add oscillation to Y-axis
            spheres[i].transform.position = currentPosition;
                       
            float scale = 1f + AudioSpectrum.audioAmp;
            spheres[i].transform.localScale = new Vector3(scale, scale, scale);
            spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);

            
            // Color Update over time
            // Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            // Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(time)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(time)); // Full saturation and brightness
            // sphereRenderer.material.color = color;
        }
    }
}
