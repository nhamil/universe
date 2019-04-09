using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float Speed = 10.0f;
    public float FastSpeed = 100.0f; 

	// Use this for initialization
	void Start () {
		
	}
	
    void FixedUpdate()
    {
        Vector3 move = Vector3.zero; 

        if (Input.GetKey(KeyCode.I))
        {
            move += Vector3.forward; 
        }
        if (Input.GetKey(KeyCode.K))
        {
            move -= Vector3.forward;
        }
        if (Input.GetKey(KeyCode.L))
        {
            move += Vector3.right;
        }
        if (Input.GetKey(KeyCode.J))
        {
            move -= Vector3.right;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            move += Vector3.up; 
        }
        if (Input.GetKey(KeyCode.Semicolon))
        {
            move -= Vector3.up; 
        }

        move.Normalize();

        if (Input.GetKey(KeyCode.Quote))
        {
            move *= FastSpeed;
        }
        else
        {
            move *= Speed;
        }
        
        transform.position += transform.rotation * move * Time.fixedDeltaTime; 
    }

}
