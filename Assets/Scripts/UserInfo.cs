using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour {

    private static GameObject player;
    private static Vector3 position;
    private static double time = 0; 

    public static GameObject Player
    {
        get { return player; } 
    }

    public static Vector3 Position
    {
        get { return position; } 
    }

    public static float Time
    {
        get { return (float) time; } 
    }

	// Use this for initialization
	void Awake () {
        player = gameObject; 
	}

    void OnDestroy()
    {
        player = null; 
    }
	
	// Update is called once per frame
	void Update () {
        position = Player.transform.position;
        time = UnityEngine.Time.time; 
	}
}
