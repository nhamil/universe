using System; 
using System.Collections;
using System.Collections.Generic;
using System.Text; 
using UnityEngine;

// https://stackoverflow.com/questions/173133/how-to-reinterpret-cast-a-float-to-an-int-is-there-a-non-static-conversion-oper
public static class ReinterpretCastExtensions 
{
   public static unsafe int AsInt( this float n ) 
   {
       return *(int*)&n;
   }
   public static unsafe long AsLong( this double n ) 
   {
       return *(long*)&n;
   } 
}

public class QuickHash 
{

    private const ulong PRIME_OFFSET = 0xCBF29CE484222325L; 
    private const ulong PRIME_MULTIPLY = 0x100000001B3L; 
    
    private QuickHash() {}
    
    public static int Compute(params byte[] data) {
        ulong hash = PRIME_OFFSET;
        
        foreach (byte b in data) {
            hash ^= b;
            hash *= PRIME_MULTIPLY; 
        }
        
        return (int) ((hash & 0xFFFFFFFF) ^ (ulong) ((uint) hash >> 32));
    }
    
    public static int Compute(String data) {
        return Compute(Encoding.ASCII.GetBytes(data));
    }
    
    public static int Compute(params short[] data) {
        ulong hash = PRIME_OFFSET;
        
        foreach (short i in data) {
            hash ^= (ulong) (ulong) ((ushort) i >> 0) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) (ulong) ((ushort) i >> 8) & 0xFF;
            hash *= PRIME_MULTIPLY; 
        }
        
        return (int) ((hash & 0xFFFFFFFF) ^ (hash >> 32));
    }
    
    public static int Compute(params int[] data) {
        ulong hash = PRIME_OFFSET;
        
        foreach (int i in data) {
            hash ^= (ulong) ((uint) i >> 0) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((uint) i >> 8) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((uint) i >> 16) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((uint) i >> 24) & 0xFF;
            hash *= PRIME_MULTIPLY; 
        }
        
        return (int) ((hash & 0xFFFFFFFF) ^ (hash >> 32));
    }
    
    public static int Compute(params long[] data) {
        ulong hash = PRIME_OFFSET;
        
        foreach (long i in data) {
            hash ^= (ulong) ((ulong) i >> 0) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 8) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 16) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 24) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 36) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 42) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 50) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 58) & 0xFF;
            hash *= PRIME_MULTIPLY; 
        }
        
        return (int) ((hash & 0xFFFFFFFF) ^ (hash >> 32));
    }
    
    public static int Compute(params float[] data) {
        ulong hash = PRIME_OFFSET;
        
        foreach (float f in data) {
            int i = f.AsInt();
            hash ^= (ulong) ((uint) i >> 0) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((uint) i >> 8) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((uint) i >> 16) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((uint) i >> 24) & 0xFF;
            hash *= PRIME_MULTIPLY; 
        }
        
        return (int) ((hash & 0xFFFFFFFF) ^ (hash >> 32));
    }
    
    public static int Compute(params double[] data) {
        ulong hash = PRIME_OFFSET;
        
        foreach (double d in data) {
            long i = d.AsLong();
            hash ^= (ulong) ((ulong) i >> 0) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 8) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 16) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 24) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 36) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 42) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 50) & 0xFF;
            hash *= PRIME_MULTIPLY; 
            hash ^= (ulong) ((ulong) i >> 58) & 0xFF;
            hash *= PRIME_MULTIPLY; 
        }
        
        return (int) ((hash & 0xFFFFFFFF) ^ (hash >> 32));
    }

}