using System; 
using UnityEngine; 

namespace SphereGen 
{
    public class MeshData 
    {
        public Vector3[] Vertices; 
        public int[] Triangles; 
        public Color[] Colors; 

        // only use on main thread 
        public void BuildMesh(Mesh mesh) 
        {
            mesh.Clear(); 
            mesh.vertices = Vertices; 
            if (Colors != null) mesh.colors = Colors; 
            if (Triangles != null) mesh.triangles = Triangles; 
            mesh.RecalculateNormals(); 
        }
    }
}