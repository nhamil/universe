using System; 
using System.Collections.Generic; 
using System.Threading; 
using UnityEngine; 

namespace SphereGen 
{

    static class MeshQueue 
    {
        private static List<LodMeshEntry> entries; 
        private static List<LodMeshEntry> completed; 
        private static List<LodMeshEntry> toCreate; 

        private static Thread[] threads;
        private static bool running = true;  

        public class LodMeshEntry : IComparable<LodMeshEntry> 
        {
            public FaceNode Node { get; private set; } 
            public bool Finished { get; internal set; } 
            public MeshData MeshData { get; internal set; } 

            public LodMeshEntry(FaceNode node) 
            {
                Node = node; 
                Finished = false; 
                MeshData = new MeshData(); 
            }

            public int CompareTo(LodMeshEntry other) 
            {
                int lod = Node.Lod - other.Node.Lod; 
                if (lod != 0) return -lod; 

                float d1 = (Node.WorldPosition - UserInfo.Position).sqrMagnitude; 
                float d2 = (Node.WorldPosition - UserInfo.Position).sqrMagnitude; 
                return (d1 < d2) ? 1 : -1; 
            }
        }

        static MeshQueue() 
        {
            entries = new List<LodMeshEntry>(); 
            completed = new List<LodMeshEntry>(); 
            toCreate = new List<LodMeshEntry>(); 

            threads = new Thread[2]; 
            for (int i = 0; i < threads.Length; i++) 
            {
                threads[i] = new Thread(new ThreadStart(ThreadProc)); 
                threads[i].Start(); 
            }
        }

        private static void ThreadProc() 
        {
            while (running) 
            {
                GenerateMesh(); 
                Thread.Sleep(1); 
            }
        }

        public static void Quit() 
        {
            running = false; 
        }

        public static LodMeshEntry RequestMeshGeneration(FaceNode node) 
        {
            LodMeshEntry entry = new LodMeshEntry(node); 
            lock (entries) 
            {
                entries.Add(entry); 
            }
            return entry; 
        }

        public static void Update() 
        {
            lock (completed) 
            {
                for (int i = 0; i < completed.Count; i++) 
                {
                    toCreate.Add(completed[i]); 
                }
                completed.Clear(); 
            }

            if (toCreate.Count == 0) return; 

            toCreate.Sort(); 
            LodMeshEntry entry = toCreate[toCreate.Count - 1]; 
            toCreate.RemoveAt(toCreate.Count - 1); 
            entry.Node.UpdateMeshData(entry.MeshData); 
        }

        private static void GenerateMesh() 
        {
            LodMeshEntry entry; 
            lock (entries) 
            {
                if (entries.Count == 0) return; 

                entries.Sort(); // TODO priority queue
                entry = entries[entries.Count - 1]; 
                entries.RemoveAt(entries.Count - 1); 
            }

            entry.Node.GenerateMesh(entry.MeshData); 
            
            lock (completed) 
            {
                completed.Add(entry); 
            }

            // Debug.Log("Created Mesh"); 
        }
    }

}