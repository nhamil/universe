using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SphereGen; 

public class SpaceBody : MonoBehaviour {

    private static GameObject AtmospherePrefab; 

	public IBodyGenerator WorldGen; 

	private FaceNode[] nodes; 

	// Use this for initialization
	void Start () 
	{
        if (AtmospherePrefab == null) AtmospherePrefab = Resources.Load("Prefabs/Atmosphere") as GameObject;
		if (WorldGen == null) WorldGen = new PlanetGenerator(1, 0); 
		float radius = 1f; 
		nodes = new FaceNode[6]; 
		nodes[0] = new FaceNode(Vector3.back, FaceIndex.Back, radius, gameObject, WorldGen); 
		nodes[1] = new FaceNode(Vector3.forward, FaceIndex.Forward, radius, gameObject, WorldGen); 
		nodes[2] = new FaceNode(Vector3.left, FaceIndex.Left, radius, gameObject, WorldGen); 
		nodes[3] = new FaceNode(Vector3.right, FaceIndex.Right, radius, gameObject, WorldGen); 
		nodes[4] = new FaceNode(Vector3.up, FaceIndex.Up, radius, gameObject, WorldGen); 
		nodes[5] = new FaceNode(Vector3.down, FaceIndex.Down, radius, gameObject, WorldGen); 
        if (WorldGen.IsPlanet && !WorldGen.IsMoon) 
        {
            GameObject atmos = Instantiate(AtmospherePrefab, Vector3.zero, Quaternion.identity);
            atmos.transform.parent = gameObject.transform; 
            atmos.transform.localPosition = Vector3.zero; 
            atmos.transform.localRotation = Quaternion.identity; 
            Material mat = atmos.GetComponent<MeshRenderer>().material; 
            Color col = ((PlanetGenerator) WorldGen).AtmosColor; 
            float str = ((PlanetGenerator) WorldGen).AtmosStrength; 
            mat.SetVector("_Color", new Vector4(col.r, col.g, col.b, 1.0f)); 
            mat.SetFloat("_Strength", str); 
			mat.SetFloat("_Radius", WorldGen.BaseHeight * 1.055f); 
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		MeshQueue.Update(); 

		foreach (FaceNode node in nodes) 
		{
			FaceNode.PreUpdate(); 
			node.Update(UserInfo.Position); 
		}
	}

	void OnDestroy() 
	{
		MeshQueue.Quit(); 
	}

}
