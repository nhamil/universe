using System; 
using UnityEngine; 

static class Noise 
{

    private static long Floor(double x) {
        long val = (long) x; 
        return val - (val > x ? 1 : 0); 
    }
    
    private static double Lerp(double a, double b, double x) {
        return a * (1 - x) + x * b; 
    }

    private static double Noise2i(long x, ulong seed) 
    {
        unchecked { return Fnv1aHash.Compute((ulong) x, seed) / (double) UInt64.MaxValue * 2 - 1; }
    }

    private static double Noise3i(long x, long y, ulong seed) 
    {
        unchecked { return Fnv1aHash.Compute((ulong) x, (ulong) y, seed) / (double) UInt64.MaxValue * 2 - 1; }
    }

    private static double Noise4i(long x, long y, long z, ulong seed) 
    {
        unchecked { return Fnv1aHash.Compute((ulong) x, (ulong) y, (ulong) z, seed) / (double) UInt64.MaxValue * 2 - 1; }
    }

    public static double GetNoise1D(double x, ulong seed) {
        long x1 = Floor(x); 
        long x2 = x1 + 1; 
        double xa = x - x1; 
        
        double v1 = Noise2i(x1, seed);
        double v2 = Noise2i(x2, seed); 
        
        return Lerp(v1, v2, xa); 
    }
    
    public static double GetNoise2D(double x, double y, ulong seed) {
        long x1 = Floor(x); 
        long x2 = x1 + 1; 
        double xa = x - x1; 
        
        long y1 = Floor(y); 
        long y2 = y1 + 1; 
        double ya = y - y1; 
        
        double v11 = Noise3i(x1, y1, seed);
        double v21 = Noise3i(x2, y1, seed);
        double v12 = Noise3i(x1, y2, seed); 
        double v22 = Noise3i(x2, y2, seed); 
        
        double i1 = Lerp(v11, v21, xa); 
        double i2 = Lerp(v12, v22, xa); 
        
        return Lerp(i1, i2, ya); 
    }
    
    public static double GetNoise3D(double x, double y, double z, ulong seed) {
        long x1 = Floor(x); 
        long x2 = x1 + 1; 
        double xa = x - x1; 
        
        long y1 = Floor(y); 
        long y2 = y1 + 1; 
        double ya = y - y1; 
        
        long z1 = Floor(z); 
        long z2 = z1 + 1; 
        double za = z - z1; 
        
        double v111 = Noise4i(x1, y1, z1, seed);
        double v211 = Noise4i(x2, y1, z1, seed);
        double v121 = Noise4i(x1, y2, z1, seed); 
        double v221 = Noise4i(x2, y2, z1, seed); 
        double v112 = Noise4i(x1, y1, z2, seed);
        double v212 = Noise4i(x2, y1, z2, seed);
        double v122 = Noise4i(x1, y2, z2, seed); 
        double v222 = Noise4i(x2, y2, z2, seed); 
        
        double i11 = Lerp(v111, v211, xa); 
        double i21 = Lerp(v121, v221, xa); 
        double i12 = Lerp(v112, v212, xa); 
        double i22 = Lerp(v122, v222, xa); 
        
        double i1 = Lerp(i11, i21, ya); 
        double i2 = Lerp(i12, i22, ya); 
        
        return Lerp(i1, i2, za); 
    }

}