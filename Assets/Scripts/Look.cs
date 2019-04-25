using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour {

    public float Sensitivity = 0.5f; 

    public float Pitch = 0.0f;
    public float Yaw = 0.0f; 

    private Quaternion velocity = Quaternion.identity; 

	// Use this for initialization
	void Start () 
    {
        Cursor.lockState = CursorLockMode.Locked; 
	}
	
	void FixedUpdate () 
    {
        // float xAmt = Input.GetAxis("Mouse X");
        // float yAmt = Input.GetAxis("Mouse Y"); 

        // Pitch = (Pitch + Sensitivity * xAmt) % 360.0f;
        // Yaw = Mathf.Clamp(Yaw - Sensitivity * yAmt, -89.0f, 89.0f); 

        // transform.rotation = Quaternion.AngleAxis(Pitch, Vector3.up) * Quaternion.AngleAxis(Yaw, Vector3.right); 
    
        float pitch = 0; 
        float yaw = 0; 
        float roll = 0; 
        float rotAmt = 0.3f * Time.fixedDeltaTime; 

        if (Input.GetKey(KeyCode.W)) 
        {
            pitch += rotAmt; 
        }
        if (Input.GetKey(KeyCode.S)) 
        {
            pitch -= rotAmt; 
        }
        if (Input.GetKey(KeyCode.A)) 
        {
            yaw -= rotAmt; 
        }
        if (Input.GetKey(KeyCode.D)) 
        {
            yaw += rotAmt; 
        }
        if (Input.GetKey(KeyCode.Q)) 
        {
            roll += rotAmt; 
        }
        if (Input.GetKey(KeyCode.E)) 
        {
            roll -= rotAmt; 
        }

        velocity *= Quaternion.AngleAxis(pitch, Vector3.right); 
        velocity *= Quaternion.AngleAxis(yaw, Vector3.up); 
        velocity *= Quaternion.AngleAxis(roll, Vector3.forward);

        if (Input.GetKey(KeyCode.C)) 
        {
            velocity = Quaternion.identity; 
        }

        transform.rotation = transform.rotation * velocity; //Quaternion.RotateTowards(transform.rotation, transform.rotation * velocity, 1); //Time.deltaTime); 
    }

    void Update() 
    {
        // TODO scale by deltaTime? 
        // transform.rotation = transform.rotation * velocity; //Quaternion.RotateTowards(transform.rotation, transform.rotation * velocity, 1); //Time.deltaTime); 
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * velocity, Time.deltaTime); 
    
        // Vector3 rot = transform.rotation.eulerAngles; 
        // Vector3 vel = velocity.eulerAngles; 
        // rot += vel * Time.deltaTime; 
        // transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z); 
    }
}
