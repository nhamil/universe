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
            planet.GetComponent<SpaceBody>().WorldGen = new PlanetGenerator(1, 0.5f); 

            if (random.NextDouble() < 0.5) 
            {
                // moons 
                int numMoons = random.Next(1, 4); 
                float moonDist = 6; 

                for (int j = 0; j < numMoons; j++) 
                {
                    float mmin = 1.2f; 
                    float mmax = 1.8f; 
                    moonDist *= (float)random.NextDouble() * (mmax - mmin) + mmin; 

                    GameObject moon = Instantiate(PlanetBase, Vector3.zero, Quaternion.identity);
                    //planet.transform.parent = star.transform;
                    Orbit morbit = moon.GetComponent<Orbit>();
                    morbit.Orbits = planet;
                    morbit.Distance = moonDist; 
                    morbit.OffsetAngle = (float)random.NextDouble() * 360.0f;
                    // TODO different types of planets 
                    moon.GetComponent<SpaceBody>().WorldGen = new PlanetGenerator(0.20f, 0.1f, true); 
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update () {

	}

}
