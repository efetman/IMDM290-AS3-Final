// UMD IMDM290 
// Instructor: Myungin Lee
// This tutorial introduce a way to draw spheres and align them in a circle with colors.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 100; 
    float time = 0f;
    Vector3[] initPos;
    // Start is called before the first frame update
    void Start()
    {
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere];
        
        foreach (GameObject sphere in spheres){
            // sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // This will cause an error. Why?
            // foreach is a read only iterator that iterates dynamically classes that implement IEnumerable, each cycle in foreach will call the IEnumerable to get the next item, the item you have is a read only reference,
        }

        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            float r = 10f; // radius of the circle
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 
            // Initial positions of the spheres. make it in circle with r radius.
            // https://www.cuemath.com/geometry/unit-circle/
            initPos[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere), 10f);
            spheres[i].transform.position = initPos[i];

            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // hsv color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // what to update?
        for (int i =0; i < numSphere; i++){
            // position
            spheres[i].transform.position = initPos[i] 
                                            + new Vector3(Mathf.Sin(time) * 5f, Mathf.Cos(time)* 3f, 1f) ;
            // color
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Sin(time)), Mathf.Cos(time), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }
}
