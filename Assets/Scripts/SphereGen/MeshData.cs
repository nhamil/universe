using System; 
using UnityEngine; 

namespace SphereGen 
{
    public class MeshData 
    {
        public Vector3[] vertices; 
        public int[] triangles; 
        public Color[] colors; 

        // only use on main thread 
        public Mesh CreateMesh() 
        {
            Mesh mesh = new Mesh(); 
            mesh.vertices = vertices; 
            if (colors != null) mesh.colors = colors; 
            if (triangles != null) mesh.triangles = triangles; 
            return mesh; 
        }
    }
}