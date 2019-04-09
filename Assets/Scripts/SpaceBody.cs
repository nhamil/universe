using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBody : MonoBehaviour {

	public IBodyGenerator WorldGen; 

	private MeshFilter[] meshFilters; 
	private SpaceBodyFace[] faces; 

	void Start() 
	{
		Vector3[] directions = 
		{
			Vector3.forward, 
			Vector3.back, 
			Vector3.left, 
			Vector3.right, 
			Vector3.up, 
			Vector3.down 
		};

		faces = new SpaceBodyFace[6]; 
		meshFilters = new MeshFilter[6]; 

		for (int i = 0; i < 6; i++) 
		{
			GameObject go = new GameObject(); 
			go.transform.parent = transform; 
			go.transform.localPosition = Vector3.zero; 
			go.transform.localRotation = Quaternion.identity; 
			go.AddComponent<MeshRenderer>().sharedMaterial = WorldGen.Material;
			meshFilters[i] = go.AddComponent<MeshFilter>(); 
			meshFilters[i].sharedMesh = new Mesh(); 

			faces[i] = new SpaceBodyFace(WorldGen, directions[i], meshFilters[i].sharedMesh); 
			faces[i].Generate(4); 
		}
	}

	void Update() 
	{

	}

}
