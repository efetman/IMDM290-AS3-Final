using UnityEngine;
using System.Collections;

public class One : MonoBehaviour
{
    void Start()
    {
        GameObject[] spheres = new GameObject[15]; 
        for (int i = 0; i < 15; i++) 
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
            spheres[i].transform.position = new Vector3(i * 1f, 0, 0); 
            
            
            Renderer renderer = spheres[i].GetComponent<Renderer>();
            switch (i % 3)
            {
                case 0: // red
                    renderer.material.color = Color.red;
                    break;
                case 1: // whitee
                    renderer.material.color = Color.white;
                    break;
                case 2: // blue
                    renderer.material.color = Color.blue;
                    break;
            }
        }
        StartCoroutine(AnimateSpheresCoroutine(spheres)); //calls main animation sequence
    }

    IEnumerator AnimateSpheresCoroutine(GameObject[] spheres)
    {
        Vector3[] finalPositions = new Vector3[15];
        for (int i = 0; i < 11; i++) // for loop sets positions for vertical line of the "1" (11 spheres)
        {
            finalPositions[i] = new Vector3(0, i * 0.75f, 0); 
        }
        
        finalPositions[14] = new Vector3(-0.75f, 7.5f, 0); //diagonal spheres
       
        finalPositions[11] = new Vector3(-1.5f, 0, 0);
        finalPositions[12] = new Vector3(-0.5f, 0, 0);
        finalPositions[13] = new Vector3(0.5f, 0, 0);
        
      
        for (int i = 0; i < spheres.Length; i++)
        {
            StartCoroutine(MoveSphereSmooth(spheres[i], finalPositions[i]));
            yield return new WaitForSeconds(0.03f);
        }
        
        yield return new WaitForSeconds(0.1f);
        
 
        for (int i = 0; i < spheres.Length; i++) //dissapear sequence
        {
            StartCoroutine(FadeAndDestroy(spheres[i]));
            yield return new WaitForSeconds(0.03f);
        }
    }

    IEnumerator MoveSphereSmooth(GameObject sphere, Vector3 targetPos)
    {
        Vector3 startPos = sphere.transform.position;
        float elapsed = 0;
        float duration = 0.2f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);
            sphere.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        sphere.transform.position = targetPos;
    }

    IEnumerator FadeAndDestroy(GameObject sphere)
    {
        Material mat = sphere.GetComponent<Renderer>().material;
        Color startColor = mat.color;
        float elapsed = 0;
        float duration = 0.1f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);
            mat.color = new Color(startColor.r, startColor.g, startColor.b, 1 - t);
            yield return null;
        }
        Destroy(sphere);
    }
}