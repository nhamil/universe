using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil 
{

    public static float Lerp(float a, float b, float x) {
        return Clamp(a, b, LerpUnsafe(a, b, x));
    }
    
    public static float LerpUnsafe(float a, float b, float x) {
        return a * (1 - x) + x * b;
    }
    
    public static float Clamp(float a, float b, float x) {
        if (b < a) {
            float tmp = b;
            b = a;
            a = tmp;
        }
        
        return Math.Min(b, Math.Max(a, x));
    }
    
    public static float ClampUnsafe(float Min, float Max, float x) {
        return Math.Min(Max, Math.Max(Min, x));
    }

    public static float ValueNoise2D(float x, float y) {
        return ValueNoise2D(x, y, 0); 
    }
    
    public static float ValueNoise3D(float x, float y, float z) {
        return ValueNoise3D(x, y, z, 0); 
    }
    
    public static float ValueNoise2D(float x, float y, int seed) {
        int x0 = (int) Math.Floor(x); 
        int y0 = (int) Math.Floor(y); 
        
        int x1 = x0 + 1; 
        int y1 = y0 + 1; 
        
        float xa = x - x0; 
        float ya = y - y0; 
        
        xa = (1 - Mathf.Cos(xa * Mathf.PI)) * 0.5f; 
        ya = (1 - Mathf.Cos(ya * Mathf.PI)) * 0.5f; 
        
        float v00 = ValueNoise2Di(seed, x0, y0); 
        float v10 = ValueNoise2Di(seed, x1, y0); 
        float v01 = ValueNoise2Di(seed, x0, y1); 
        float v11 = ValueNoise2Di(seed, x1, y1); 
        
        float i0 = LerpUnsafe(v00, v10, xa); 
        float i1 = LerpUnsafe(v01, v11, xa); 
        
        return LerpUnsafe(i0, i1, ya); 
    }
    
    public static float ValueNoise3D(float x, float y, float z, int seed) {
        int x0 = (int) Math.Floor(x); 
        int y0 = (int) Math.Floor(y); 
        int z0 = (int) Math.Floor(z); 
        
        int x1 = x0 + 1; 
        int y1 = y0 + 1; 
        int z1 = z0 + 1; 
        
        float xa = x - x0; 
        float ya = y - y0; 
        float za = z - z0; 
        
        xa = (1 - Mathf.Cos(xa * Mathf.PI)) * 0.5f; 
        ya = (1 - Mathf.Cos(ya * Mathf.PI)) * 0.5f; 
        za = (1 - Mathf.Cos(za * Mathf.PI)) * 0.5f; 
        
        float v000 = ValueNoise3Di(seed, x0, y0, z0); 
        float v100 = ValueNoise3Di(seed, x1, y0, z0); 
        float v010 = ValueNoise3Di(seed, x0, y1, z0); 
        float v110 = ValueNoise3Di(seed, x1, y1, z0); 
        float v001 = ValueNoise3Di(seed, x0, y0, z1); 
        float v101 = ValueNoise3Di(seed, x1, y0, z1); 
        float v011 = ValueNoise3Di(seed, x0, y1, z1); 
        float v111 = ValueNoise3Di(seed, x1, y1, z1); 
        
        float i00 = LerpUnsafe(v000, v100, xa); 
        float i10 = LerpUnsafe(v010, v110, xa); 
        float i01 = LerpUnsafe(v001, v101, xa); 
        float i11 = LerpUnsafe(v011, v111, xa); 
        
        float i0 = LerpUnsafe(i00, i10, ya); 
        float i1 = LerpUnsafe(i01, i11, ya); 
        
        return LerpUnsafe(i0, i1, za); 
    }
    
    private static float ValueNoise2Di(int seed, int x, int y) {
        return ClampUnsafe(-1, 1, 1 * (float) ((double) QuickHash.Compute(seed, x, y) / Int32.MaxValue) - 0); 
    }
    
    private static float ValueNoise3Di(int seed, int x, int y, int z) {
        return ClampUnsafe(-1, 1, 1 * (float) ((double) QuickHash.Compute(seed, x, y, z) / Int32.MaxValue) - 0); 
    }

}