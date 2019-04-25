using System; 
using System.Collections.Generic; 
using System.Threading; 
using UnityEngine; 

namespace SphereGen 
{

    static class MeshQueue 
    {
        private static List<LodMeshEntry> entries; // used on both threads
        private static List<LodMeshEntry> completed; // used on both threads 
        private static List<LodMeshEntry> toCreate; 

        private static Thread[] threads;
        private static bool running = true;  

        private static object positionLock = new object(); 
        private static Vector3 userPosition = Vector3.zero; 
        private static Vector3 workerUserPosition = Vector3.zero; 
        
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

                float d1 = (Node.WorldPosition - workerUserPosition).sqrMagnitude; 
                float d2 = (other.Node.WorldPosition - workerUserPosition).sqrMagnitude; 
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
                lock (positionLock) 
                {
                    workerUserPosition = userPosition; 
                }

                GenerateMesh(); 
                Thread.Sleep(1); 
            }
        }

        public static void Quit() 
        {
            running = false; 
        }

        // worker thread 
        public static LodMeshEntry RequestMeshGeneration(FaceNode node) 
        {
            LodMeshEntry entry = new LodMeshEntry(node); 
            lock (entries) 
            {
                entries.Add(entry); 
            }
            return entry; 
        }

        private static void RemoveNodeFromList(FaceNode node, List<LodMeshEntry> list) 
        {
            for (int i = 0; i < list.Count; i++) 
            {
                if (list[i].Node == node) 
                {
                    list.RemoveAt(i); 
                    return; 
                }
            }
        }

        // main thread 
        public static void OnDestroy(FaceNode node) 
        {
            lock (entries) 
            {
                RemoveNodeFromList(node, entries); 
            }

            lock (completed) 
            {
                RemoveNodeFromList(node, completed); 
            }

            RemoveNodeFromList(node, toCreate); 
        }

        // main thread 
        public static void Update() 
        {
            FaceNode.PreUpdate(); 

            lock (positionLock) 
            {
                userPosition = UserInfo.Position; 
            }

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

        // worker thread 
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