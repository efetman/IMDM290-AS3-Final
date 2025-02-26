using UnityEngine;
public class wisps : MonoBehaviour
{
    public int numberOfSpheres = 50;
    public float totalDisappearTime = 0.3f;
    public float waveSpeed = 15f;
    public float waveHeight = 2f;
    private GameObject[] spheres;
    private float startTime;
    
    private bool hasStarted = false;

    void Start()
    {
        spheres = new GameObject[numberOfSpheres];
        

        Material redMaterial = new Material(Shader.Find("Standard"));
        redMaterial.color = Color.red;
        
        Material whiteMaterial = new Material(Shader.Find("Standard"));
        whiteMaterial.color = Color.white;
        
        Material blueMaterial = new Material(Shader.Find("Standard"));
        blueMaterial.color = Color.blue;

        for (int i = 0; i < numberOfSpheres; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.position = new Vector3(i * 0.2f, 0, -20f);
            spheres[i].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f - 20f);
            

            if (i % 3 == 0)
                spheres[i].GetComponent<Renderer>().material = redMaterial;
            else if (i % 3 == 1)
                spheres[i].GetComponent<Renderer>().material = whiteMaterial;
            else
                spheres[i].GetComponent<Renderer>().material = blueMaterial;
        }
        startTime = Time.time;
        hasStarted = true;
    }

    void Update()
    {
        float timeSinceStart = Time.time - startTime;
        // for loop for wave motion
        for (int i = 0; i < numberOfSpheres; i++)
        {
            if (spheres[i] != null)
            {
                
                float wave = Mathf.Sin((Time.time * waveSpeed) + (i * 0.2f)) * waveHeight;
                spheres[i].transform.position = new Vector3(i * 0.2f, wave, 0);
                
                if (timeSinceStart >= (i * totalDisappearTime / numberOfSpheres))
                {
                    Destroy(spheres[i]);
                }
            }
        }
    }
}
