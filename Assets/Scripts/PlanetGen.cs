using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SphereGen; 

public class PlanetGen : MonoBehaviour 
{

	private FaceNode[] nodes; 

	// Use this for initialization
	void Start () 
	{
		float radius = 1f; 
		nodes = new FaceNode[6]; 
		nodes[0] = new FaceNode(Vector3.back, FaceIndex.Back, radius); 
		nodes[1] = new FaceNode(Vector3.forward, FaceIndex.Forward, radius); 
		nodes[2] = new FaceNode(Vector3.left, FaceIndex.Left, radius); 
		nodes[3] = new FaceNode(Vector3.right, FaceIndex.Right, radius); 
		nodes[4] = new FaceNode(Vector3.up, FaceIndex.Up, radius); 
		nodes[5] = new FaceNode(Vector3.down, FaceIndex.Down, radius); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		MeshQueue.Update(); 

		foreach (FaceNode node in nodes) 
		{
			node.Update(UserInfo.Position); 
		}
	}
}
