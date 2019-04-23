using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SphereGen; 

public class SpaceBody : MonoBehaviour {

	public IBodyGenerator WorldGen; 

	private FaceNode[] nodes; 

	// Use this for initialization
	void Start () 
	{
		if (WorldGen == null) WorldGen = new PlanetGenerator(); 
		float radius = 1f; 
		nodes = new FaceNode[6]; 
		nodes[0] = new FaceNode(Vector3.back, FaceIndex.Back, radius, gameObject, WorldGen); 
		nodes[1] = new FaceNode(Vector3.forward, FaceIndex.Forward, radius, gameObject, WorldGen); 
		nodes[2] = new FaceNode(Vector3.left, FaceIndex.Left, radius, gameObject, WorldGen); 
		nodes[3] = new FaceNode(Vector3.right, FaceIndex.Right, radius, gameObject, WorldGen); 
		nodes[4] = new FaceNode(Vector3.up, FaceIndex.Up, radius, gameObject, WorldGen); 
		nodes[5] = new FaceNode(Vector3.down, FaceIndex.Down, radius, gameObject, WorldGen); 
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

	void OnDestroy() 
	{
		MeshQueue.Quit(); 
	}

}
