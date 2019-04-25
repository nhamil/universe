using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {

    public GameObject Orbits;
    public float Distance;
    public float OffsetAngle;

    private float orbitalPeriod; 
    private float currentAngle;
    
    // Use this for initialization
    void Start () {
        orbitalPeriod = 2 * Mathf.PI * Mathf.Sqrt(Distance * Distance * Distance / (5000)) * 1000;

        SetPosition(); 
	}
	
	// Update is called once per frame
	void Update () {
        SetPosition(); 
	}

    private void SetPosition()
    {
        transform.position = Orbits.transform.position + Quaternion.AngleAxis(OffsetAngle + UserInfo.Time / orbitalPeriod, Vector3.up) * new Vector3(0, 0, Distance);
    }
}
