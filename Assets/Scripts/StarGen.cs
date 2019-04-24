using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGen : MonoBehaviour {

    private GameObject StarBase;
    private GameObject PlanetBase; 
    
	// Use this for initialization
	void Awake () {
        StarBase = Resources.Load("Prefabs/Star") as GameObject;
        PlanetBase = Resources.Load("Prefabs/Planet") as GameObject; 
    }

    void Start()
    {
        System.Random random = new System.Random();
        
        GameObject star = Instantiate(StarBase, Vector3.zero, Quaternion.identity);
        star.GetComponent<SpaceBody>().WorldGen = new StarGenerator(); 
        RenderSettings.sun = star.GetComponent<Light>(); 

        int numPlanets = random.Next(10, 13);
        float distance = 45; 

        for (int i = 0; i < numPlanets; i++)
        {
            float min = 1.2f; 
            float max = 1.5f; 
            distance *= (float)random.NextDouble() * (max - min) + min; 

            GameObject planet = Instantiate(PlanetBase, Vector3.zero, Quaternion.identity);
            //planet.transform.parent = star.transform;
            Orbit orbit = planet.GetComponent<Orbit>();
            orbit.Orbits = star;
            orbit.Distance = distance; 
            orbit.OffsetAngle = (float)random.NextDouble() * 360.0f;
            // TODO different types of planets 
            planet.GetComponent<SpaceBody>().WorldGen = new PlanetGenerator(); 
        }
    }
    
    // Update is called once per frame
    void Update () {

	}

}
