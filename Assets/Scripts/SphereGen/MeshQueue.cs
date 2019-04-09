using System; 
using System.Collections.Generic; 
using System.Threading; 
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

    static class MeshQueue 
    {
        private static List<LodMeshEntry> entries; 

        public class LodMeshEntry : IComparable<LodMeshEntry> 
        {
            public FaceNode Node { get; private set; } 
            public bool Finished { get; internal set; } 
            public MeshData MeshData { get; internal set; } 

            public LodMeshEntry(FaceNode node) 
            {
                Node = node; 
                Finished = false; 
                MeshData = null; 
            }

            public int CompareTo(LodMeshEntry other) 
            {
                return Node.Lod - other.Node.Lod; 
            }
        }

        static MeshQueue() 
        {
            entries = new List<LodMeshEntry>(); 
        }

        public static LodMeshEntry RequestMeshGeneration(FaceNode node) 
        {
            LodMeshEntry entry = new LodMeshEntry(node); 
            entries.Add(entry); 
            entries.Sort(); // TODO priority queue
            return entry; 
        }

        public static void Update() 
        {
            if (entries.Count == 0) return; 

            LodMeshEntry entry = entries[0]; 
            entries.RemoveAt(0); 

            entry.Node.GenerateMesh(); 
            entry.Finished = true; 
        }
    }

}