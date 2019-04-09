using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBodyFace 
{
    private IBodyGenerator worldGen; 
    private Vector3 position; 
    private Mesh mesh; 

    public SpaceBodyFace(IBodyGenerator worldGen, Vector3 position, Mesh mesh) 
    {
        this.worldGen = worldGen; 
        this.position = position; 
        this.mesh = mesh; 
    }

    public void Generate(int lod) 
    {
        int size = 32 * (lod + 1); 

        // Debug.Log("Face Size: " + size + ", " + size * size); 

        Vector3 up = new Vector3(position.y, position.z, position.x); 
        Vector3 right = new Vector3(position.z, position.x, position.y); 

        // make sure the winding will be correct 
        if (Vector3.Dot(Vector3.Cross(up, right), position) > 0) up *= -1; 

        int[] indices = new int[size * size * 6]; 
        Vector3[] positions = new Vector3[size * size]; 
        Vector3[] normals = new Vector3[size * size]; 
        Color[] colors = new Color[size * size]; 

        for (int y = 0; y < size; y++) 
        {
            float yAmt = (float) y / (size - 1) * 2 - 1; 
            for (int x = 0; x < size; x++) 
            {
                float xAmt = (float) x / (size - 1) * 2 - 1; 
                Vector3 pos = (position + xAmt * right + yAmt * up).normalized; 
                float height = worldGen.GetHeight(pos, lod); 
                pos *= height; 
                positions[x + y * size] = pos; 

                if (height > worldGen.BaseHeight) 
                {
                    colors[x + y * size] = new Color(0.0f, 1.0f, 0.0f); 
                }
                else 
                {
                    colors[x + y * size] = new Color(0.0f, 0.0f, 1.0f); 
                }
            }
        }

        // for (int y = 0; y < size; y++) 
        // {
        //     float yAmt = (float) y / (size - 1) * 2 - 1; 
        //     for (int x = 0; x < size; x++) 
        //     {
        //         float xAmt = (float) x / (size - 1) * 2 - 1; 

        //         Vector3 a = positions[x + y * size]; 
        //         Vector3 b = x + 1 >= size ? worldGen.GetPosition((position + ((x + 1.0f) / (size - 1) * 2 - 1) * right + yAmt * up).normalized) : positions[(x + 1) + y * size]; 
        //         Vector3 c = y + 1 >= size ? worldGen.GetPosition((position + xAmt * right + ((y + 1.0f) / (size - 1) * 2 - 1) * up).normalized) : positions[x + (y + 1) * size]; 
        //         Vector3 d = x - 1 < 0 ? worldGen.GetPosition((position + ((x - 1.0f) / (size - 1) * 2 - 1) * right + yAmt * up).normalized) : positions[(x - 1) + y * size]; 
        //         Vector3 e = y - 1 < 0 ? worldGen.GetPosition((position + xAmt * right + ((y - 1.0f) / (size - 1) * 2 - 1) * up).normalized) : positions[x + (y - 1) * size]; 

        //         int ind = x + y * size; 
        //         normals[ind] = Vector3.Cross((b - a).normalized, (c - a).normalized); 
        //         normals[ind] += Vector3.Cross((a - d).normalized, (c - a).normalized); 
        //         normals[ind] += Vector3.Cross((b - a).normalized, (a - e).normalized); 
        //         normals[ind] += Vector3.Cross((a - d).normalized, (a - e).normalized); 
        //         normals[ind] /= 4; 
        //     }
        // }

        int i = 0; 
        for (int y = 0; y < size - 1; y++) 
        {
            for (int x = 0; x < size - 1; x++) 
            {
                indices[i++] = (x) + (y) * size; 
                indices[i++] = (x + 1) + (y) * size; 
                indices[i++] = (x + 1) + (y + 1) * size; 

                indices[i++] = (x) + (y) * size; 
                indices[i++] = (x + 1) + (y + 1) * size; 
                indices[i++] = (x) + (y + 1) * size; 
            }
        }

        mesh.Clear(); 
        mesh.vertices = positions; 
        mesh.colors = colors; 
        // mesh.normals = normals; 
        mesh.triangles = indices; 
        mesh.RecalculateNormals(); 
    }

    public void Destroy() 
    {
        // TODO 
    }
}