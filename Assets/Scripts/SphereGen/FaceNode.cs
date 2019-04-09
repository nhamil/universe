using System; 
using UnityEngine; 

namespace SphereGen 
{

    public class FaceNode //: IQuadtree 
    {
        private FaceNode[] children; 
        private bool meshEnabled = false; 

        ////////////////////////////////////////////////////////

        public FaceIndex FaceIndex { get; private set; }

        public FaceNode Parent { get; private set; } 

        public int Lod { get; private set; } 

        public Vector3 Position { get; private set; } 

        public float Radius { get; private set; } 

        public MeshFilter MeshFilter { get; private set; } 

        public bool MeshEnabled 
        { 
            get { return meshEnabled; } 
            private set 
            {
                if (value == meshEnabled) return; 
                meshEnabled = value; 
                if (MeshFilter != null) MeshFilter.gameObject.GetComponent<MeshRenderer>().enabled = meshEnabled; 
            } 
        } 

        ////////////////////////////////////////////////////////

        public bool HasChildren { get { return children != null; } } 

        ////////////////////////////////////////////////////////

        private static Material material;

        public FaceNode(Vector3 pos, FaceIndex index, float radius) 
        {
            if (material == null) 
            {
                material = Resources.Load("Materials/PlanetBaseMaterial") as Material; 
            }
            Parent = null; 
            Lod = 0; 
            Position = pos; 
            FaceIndex = index; 
            Radius = radius; 
            // GenerateMesh(); 
            MeshQueue.RequestMeshGeneration(this); 
        }

        private FaceNode(FaceNode parent, Vector3 pos) 
        {
            Parent = parent; 
            Lod = parent.Lod + 1; 
            Position = pos; 
            FaceIndex = parent.FaceIndex; 
            Radius = parent.Radius * 0.5f; 
            // GenerateMesh(); 
            MeshQueue.RequestMeshGeneration(this); 
        }

        public void Update(Vector3 camPos) 
        {
            if (Parent != null) 
            {
                if (Parent.MeshEnabled) 
                {
                    MeshEnabled = false; 
                }
            }
            else 
            {
                MeshEnabled = true; 
            }

            float distance = (Position.normalized - camPos).magnitude; 
            float shrinkDist = 5 * Radius; 
            float expandDist = 4 * Radius; 

            if (HasChildren) 
            {
                bool allChildMeshes = true; 
                MeshEnabled = true; 
                foreach (FaceNode node in children) 
                {
                    if (node.MeshFilter == null) allChildMeshes = false; 
                    node.Update(camPos); 
                }
                MeshEnabled = !allChildMeshes; 
            }

            if (distance > shrinkDist) 
            {
                if (HasChildren) 
                {
                    DestroyChildrenIfNeeded(); 
                }
            }
            else if (distance < expandDist) 
            {
                if (MeshFilter != null && !HasChildren && Lod < 8) 
                {
                    CreateChildren(); 
                }
            }
        }

        private void CreateMesh() 
        {
            GameObject go = new GameObject(); 
            if (Parent != null) 
            {
                go.transform.parent = Parent.MeshFilter.transform; 
                go.transform.position = Vector3.zero; 
                go.transform.rotation = Quaternion.identity; 
            }
            go.AddComponent<MeshRenderer>().sharedMaterial = material;
			MeshFilter = go.AddComponent<MeshFilter>(); 
            MeshEnabled = false; 
            MeshFilter.sharedMesh = new Mesh(); 
        }

        public void GenerateMesh() 
        {
            if (MeshFilter != null) throw new System.Exception("Mesh is already generated, cannot generate mesh");  

            CreateMesh(); 
            Mesh mesh = MeshFilter.sharedMesh; 
            // Vector3[] verts = new Vector3[4]; 
            // int[] inds = new int[] { 0, 1, 2, 0, 2, 3 }; 

            // verts[0] = Position - FaceIndex.GetRight() + FaceIndex.GetUp(); 
            // verts[1] = Position - FaceIndex.GetRight() - FaceIndex.GetUp(); 
            // verts[2] = Position + FaceIndex.GetRight() - FaceIndex.GetUp(); 
            // verts[3] = Position + FaceIndex.GetRight() + FaceIndex.GetUp(); 

            // mesh.vertices = verts; 
            // mesh.triangles = inds; 
            // mesh.RecalculateNormals(); 

            int size = 64; 

            IBodyGenerator worldGen = new PlanetGenerator(); 

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
                    float height = worldGen.GetHeight(pos, Lod); 
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

            MeshEnabled = false; 
        }

        private void DestroySelf() 
        {
            if (MeshFilter != null) UnityEngine.Object.Destroy(MeshFilter.gameObject); 
            MeshFilter = null; 
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
                MeshEnabled = true; 
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