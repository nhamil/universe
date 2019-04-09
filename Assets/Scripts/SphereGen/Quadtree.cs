using System; 
using UnityEngine; 

namespace SphereGen 
{

    public enum FaceIndex 
    {
        Forward, 
        Right, 
        Back, 
        Left, 
        Up, 
        Down 
    }

    public enum QuadTreeDirection 
    {
        Up, 
        Down, 
        Left, 
        Right 
    }

    public enum QuadtreePosition 
    {
        UpLeft, 
        UpRight, 
        DownLeft, 
        DownRight 
    }

    // public interface IQuadtree 
    // {
    //     IQuadtree Parent { get; } 

    //     int Lod { get; } 

    //     FaceIndex FaceIndex { get; } 

    //     bool HasChildren { get; } 

    //     Vector3 Position { get; } 

    //     void Update(Vector3 camPosition); 

    //     IQuadtree GetNeighbor(QuadTreeDirection direction, FaceIndex source); 

    //     IQuadtree GetChild(QuadtreePosition position); 
    // }

    static class EnumUtil 
    {
        public static int GetIndex(this QuadtreePosition pos) 
        {
            return (int) pos; 
        }

        public static int GetIndex(this FaceIndex index) 
        {
            return (int) index; 
        }

        public static Vector3 GetRight(this FaceIndex index) 
        {
            switch (index) 
            {
                case FaceIndex.Forward: return Vector3.right; 
                case FaceIndex.Right: return Vector3.back; 
                case FaceIndex.Back: return Vector3.left; 
                case FaceIndex.Left: return Vector3.forward; 
                case FaceIndex.Up: return Vector3.right; 
                case FaceIndex.Down: return Vector3.right; 
                default: throw new System.Exception("Invalid FaceIndex: Cannot get right direction"); 
            }
        }

        public static Vector3 GetUp(this FaceIndex index) 
        {
            switch (index) 
            {
                case FaceIndex.Forward: return Vector3.up; 
                case FaceIndex.Right: return Vector3.up; 
                case FaceIndex.Back: return Vector3.up; 
                case FaceIndex.Left: return Vector3.up; 
                case FaceIndex.Up: return Vector3.back; 
                case FaceIndex.Down: return Vector3.forward; 
                default: throw new System.Exception("Invalid FaceIndex: Cannot get up direction"); 
            }
        }

        public static Vector3 GetForward(this FaceIndex index) 
        {
            switch (index) 
            {
                case FaceIndex.Forward: return Vector3.forward; 
                case FaceIndex.Right: return Vector3.right; 
                case FaceIndex.Back: return Vector3.back; 
                case FaceIndex.Left: return Vector3.left; 
                case FaceIndex.Up: return Vector3.up; 
                case FaceIndex.Down: return Vector3.down; 
                default: throw new System.Exception("Invalid FaceIndex: Cannot get forward direction"); 
            }
        }
    }

}