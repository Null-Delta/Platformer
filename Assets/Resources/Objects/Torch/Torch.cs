using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;


public class Torch : MapObject
{

    public override string objectName => "Torch";

    [SerializeField] float flickersPerSecond = 15f;
    [SerializeField] float flickerRangeMin = -0.1f;
    [SerializeField] float flickerRangeMax = 0.1f;
    Light2D light2D;
    float intensity;
    float time;

    public override void updateObject()
    {
        if ((Time.realtimeSinceStartup - time) * 1000 > 1000f / flickersPerSecond) 
	    {
            float newIntensity = intensity + Random.Range(flickerRangeMin, flickerRangeMax);
            light2D.intensity = newIntensity;
            time = Time.realtimeSinceStartup;
        }
    }

    public override void startObject()
    {
        base.startObject();
        order = ObjectOrder.wall;
        gameObject.transform.position = position;
        
        light2D = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Light2D>();
        intensity = light2D.intensity;
        time = Time.realtimeSinceStartup;
    }

    public Torch(int x, int y) : base(x, y)
    {
        isCollisiable = false;
    }
}
