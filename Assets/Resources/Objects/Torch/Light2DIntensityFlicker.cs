using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Light2DIntensityFlicker : MonoBehaviour
{
	public float flickersPerSecond = 15f;
        public float flickerRangeMin = -0.1f;
        public float flickerRangeMax = 0.1f;

	private Light2D light2D;
	private float intensity;
    private float time;

	private void Start() 
	{
		light2D = GetComponent<Light2D>();
		intensity = light2D.intensity;
	}

	private void Update() 
	{
            if (GetMillisecs() > 1000f / flickersPerSecond) 
	    {
                float newIntensity = intensity + Random.Range(flickerRangeMin, flickerRangeMax);
				light2D.intensity = newIntensity;
                ResetTime();
            }
        }

	private float GetMillisecs() 
	{
		return (Time.realtimeSinceStartup - time) * 1000;
	}

	public void ResetTime() 
	{
		time = Time.realtimeSinceStartup;
	}
}