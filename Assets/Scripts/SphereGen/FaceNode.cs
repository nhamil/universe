using System; 
using UnityEngine; 

namespace SphereGen 
{

    public class FaceNode //: IQuadtree 
    {
        private FaceNode[] children; 
        // private bool meshEnabled = false; 

        ////////////////////////////////////////////////////////

        public FaceIndex FaceIndex { get; private set; }

        public FaceNode Parent { get; private set; } 

        public int Lod { get; private set; } 

        public int MaxLod { get { return WorldGen.IsPlanet ? 4 : 2; } }

        public Vector3 Position { get; private set; } 

        public Vector3 WorldPosition { get; private set; } 

        public float Radius { get; private set; } 

        public GameObject GameObject { get; private set; } 

        public GameObject ParentGameObject { get; private set; } 

        public MeshRenderer MeshRenderer { get; private set; } 

        public MeshFilter MeshFilter { get; private set; } 

        public bool HasUpdatedMesh { get; private set; } 

        public IBodyGenerator WorldGen { get; private set; } 

        private bool IsDestroyed { get; set; }

        // public bool MeshEnabled 
        // { 
        //     get { return meshEnabled; } 
        //     private set 
        //     {
        //         if (value == meshEnabled) return; 
        //         meshEnabled = value; 
        //         if (MeshFilter != null) MeshFilter.gameObject.GetComponent<MeshRenderer>().enabled = meshEnabled; 
        //     } 
        // } 

        ////////////////////////////////////////////////////////

        public bool HasChildren { get { return children != null; } } 

        ////////////////////////////////////////////////////////

        private static Material material;
        private static Material starMaterial; 

        public FaceNode(Vector3 pos, FaceIndex index, float radius, GameObject parentGameObject, IBodyGenerator worldGen) 
        {
            if (material == null) 
            {
                material = Resources.Load("Materials/PlanetBaseMaterial") as Material; 
                starMaterial = Resources.Load("Materials/StarBaseMaterial") as Material; 
            }
            ParentGameObject = parentGameObject; 
            WorldGen = worldGen; 
            Parent = null; 
            Lod = 0; 
            Position = pos; 
            FaceIndex = index; 
            Radius = radius; 
            HasUpdatedMesh = false; 
            CreateGameObject(); 
            // GenerateMesh(); 
            MeshQueue.RequestMeshGeneration(this); 
        }

        private FaceNode(FaceNode parent, Vector3 pos) 
        {
            Parent = parent; 
            ParentGameObject = parent.ParentGameObject; 
            WorldGen = parent.WorldGen; 
            Lod = parent.Lod + 1; 
            Position = pos; 
            FaceIndex = parent.FaceIndex; 
            Radius = parent.Radius * 0.5f; 
            CreateGameObject(); 
            // GenerateMesh(); 
            MeshQueue.RequestMeshGeneration(this); 
        }

        private void SetMeshRendererEnabled(bool enabled) 
        {
            if (IsDestroyed) return; 
            if (enabled && !MeshRenderer.enabled) 
            {
                MeshRenderer.enabled = true; 
            }
            else if (!enabled && MeshRenderer.enabled) 
            {
                MeshRenderer.enabled = false; 
            }
        }

        public void Update(Vector3 camPos) 
        {
            if (IsDestroyed) 
            {
                return; 
            }

            bool shouldRender = false; 

            if (Parent != null) // has parent 
            {
                shouldRender = !Parent.MeshRenderer.enabled; 
            }
            else // is root 
            {
                shouldRender = true; 
            }

            WorldPosition = Position.normalized + GameObject.transform.position; 
            float distance = (WorldPosition - camPos).magnitude; 
            float shrinkDist = 20 * Radius; 
            float expandDist = 12 * Radius; 

            if (distance > shrinkDist) 
            {
                if (HasChildren) 
                {
                    DestroyChildrenIfNeeded(); 
                }
            }

            if (HasChildren) 
            {
                bool allChildMeshes = true; 
                foreach (FaceNode node in children) 
                {
                    if (!node.HasUpdatedMesh) 
                    {
                        allChildMeshes = false; 
                        break; 
                    }
                }
                if (shouldRender) shouldRender = !allChildMeshes; // if was going to render, but all children are generated, render them instead 
            }

            SetMeshRendererEnabled(shouldRender); 

            if (HasChildren) 
            {
                foreach (FaceNode node in children) 
                {
                    node.Update(camPos); 
                }
            }

            if (distance < expandDist) 
            {
                if (MeshFilter != null && !HasChildren && Lod < MaxLod) 
                {
                    CreateChildren(); 
                }
            }
        }

        private void CreateGameObject() 
        {
            GameObject = new GameObject(); 
            if (ParentGameObject != null) 
            {
                GameObject.transform.parent = ParentGameObject.transform; 
                GameObject.transform.localPosition = Vector3.zero; 
                GameObject.transform.localRotation = Quaternion.identity; 
            }
            MeshRenderer = GameObject.AddComponent<MeshRenderer>(); 
            MeshRenderer.sharedMaterial = WorldGen.IsPlanet ? material : starMaterial;
			MeshFilter = GameObject.AddComponent<MeshFilter>(); 
            MeshFilter.sharedMesh = new Mesh(); 
        }

        public void UpdateMeshData(MeshData meshData) 
        {
            if (IsDestroyed) return; 
            HasUpdatedMesh = true; 
            Mesh mesh = MeshFilter.sharedMesh; 
            meshData.BuildMesh(mesh); 
        }

        public void GenerateMesh(MeshData mesh) 
        {
            int size = 64; 

            Vector3 position = Position; 
            Vector3 up = FaceIndex.GetUp() * Radius; 
            Vector3 right = FaceIndex.GetRight() * Radius; 

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
                    float height = WorldGen.GetHeight(pos, Lod); 
                    pos *= height; 
                    positions[x + y * size] = pos; 

                    if (WorldGen.IsPlanet) 
                    {
                        if (height > WorldGen.BaseHeight) 
                        {
                            colors[x + y * size] = WorldGen.LowLandColor; 
                        }
                        else // is water 
                        {
                            colors[x + y * size] = WorldGen.WaterColor; 
                        }
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
            //         Vector3 b = x + 1 >= size ? WorldGen.GetPosition((position + ((x + 1.0f) / (size - 1) * 2 - 1) * right + yAmt * up).normalized) : positions[(x + 1) + y * size]; 
            //         Vector3 c = y + 1 >= size ? WorldGen.GetPosition((position + xAmt * right + ((y + 1.0f) / (size - 1) * 2 - 1) * up).normalized) : positions[x + (y + 1) * size]; 
            //         Vector3 d = x - 1 < 0 ? WorldGen.GetPosition((position + ((x - 1.0f) / (size - 1) * 2 - 1) * right + yAmt * up).normalized) : positions[(x - 1) + y * size]; 
            //         Vector3 e = y - 1 < 0 ? WorldGen.GetPosition((position + xAmt * right + ((y - 1.0f) / (size - 1) * 2 - 1) * up).normalized) : positions[x + (y - 1) * size]; 

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

            mesh.Vertices = positions; 
            if (WorldGen.IsPlanet) mesh.Colors = colors; 
            // mesh.Normals = normals; 
            mesh.Triangles = indices; 
        }

        private void DestroySelf() 
        {
            if (GameObject != null) UnityEngine.Object.Destroy(GameObject); 
            GameObject = null; 
            IsDestroyed = true; 
            HasUpdatedMesh = false; 
            DestroyChildrenIfNeeded(); 
            // TODO destroy 
        }

        private void DestroyChildrenIfNeeded() 
        {
            if (HasChildren) 
            {
                foreach (FaceNode node in children) 
                {
                    node.DestroySelf(); 
                }
                children = null; 
            }
        }

        private void CreateChildren() 
        {
            if (HasChildren) 
            {
                throw new System.Exception("Attempted to create children on quad tree node that already had children"); 
            }
            
            float newRad = Radius * 0.5f; 
            
            Vector3 right = FaceIndex.GetRight(); 
            Vector3 up = FaceIndex.GetUp(); 

            children = new FaceNode[4]; 
            children[QuadtreePosition.UpLeft.GetIndex()] = new FaceNode(this, Position - right * newRad + up * newRad); 
            children[QuadtreePosition.UpRight.GetIndex()] = new FaceNode(this, Position + right * newRad + up * newRad); 
            children[QuadtreePosition.DownLeft.GetIndex()] = new FaceNode(this, Position - right * newRad - up * newRad); 
            children[QuadtreePosition.DownRight.GetIndex()] = new FaceNode(this, Position + right * newRad - up * newRad); 
        }

        public FaceNode GetNeighbor(QuadTreeDirection direction) 
        {
            return null; 
        }

        public FaceNode GetChild(QuadtreePosition position) 
        {
            return children[position.GetIndex()]; 
        }
    }

}