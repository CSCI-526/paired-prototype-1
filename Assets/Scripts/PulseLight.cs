using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PulseLight : MonoBehaviour
{
    public Light2D light2D;
    public float originalIntensity = 0.4f;
    public float amplitude = 0.15f;
    public float speed = 2f;


    // Initialize the light2D reference.
    void Reset()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }
    }

    // Make the light pulse.
    void Update()
    {
        if (!light2D) return;

        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        light2D.intensity = originalIntensity + amplitude * t;
        
    }
}

