using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour {

    public float Sensitivity = 0.5f; 

    public float Pitch = 0.0f;
    public float Yaw = 0.0f; 

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked; 
	}
	
	void FixedUpdate () {
        float xAmt = Input.GetAxis("Mouse X");
        float yAmt = Input.GetAxis("Mouse Y"); 

        Pitch = (Pitch + Sensitivity * xAmt) % 360.0f;
        Yaw = Mathf.Clamp(Yaw - Sensitivity * yAmt, -89.0f, 89.0f); 

        transform.rotation = Quaternion.AngleAxis(Pitch, Vector3.up) * Quaternion.AngleAxis(Yaw, Vector3.right); 
	}
}
