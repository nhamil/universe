static class Fnv1aHash 
{

    public const ulong Prime = 1099511628211L; 
    public const ulong Offset = 0xcbf29ce484222325L; 

    public static ulong Compute(ulong a) {
        ulong hash = Offset; 
        
        hash ^= a & 255; 
        hash *= Prime; 
        hash ^= (a >> 8) & 255; 
        hash *= Prime; 
        hash ^= (a >> 16) & 255; 
        hash *= Prime; 
        hash ^= (a >> 24) & 255; 
        hash *= Prime; 
        hash ^= (a >> 32) & 255; 
        hash *= Prime; 
        hash ^= (a >> 40) & 255; 
        hash *= Prime; 
        hash ^= (a >> 48) & 255; 
        hash *= Prime; 
        hash ^= (a >> 56) & 255; 
        hash *= Prime; 

        return hash; 
    }
    
    public static ulong Compute(ulong a, ulong b) {
        ulong hash = Offset; 
        
        hash ^= a & 255; 
        hash *= Prime; 
        hash ^= (a >> 8) & 255; 
        hash *= Prime; 
        hash ^= (a >> 16) & 255; 
        hash *= Prime; 
        hash ^= (a >> 24) & 255; 
        hash *= Prime; 
        hash ^= (a >> 32) & 255; 
        hash *= Prime; 
        hash ^= (a >> 40) & 255; 
        hash *= Prime; 
        hash ^= (a >> 48) & 255; 
        hash *= Prime; 
        hash ^= (a >> 56) & 255; 
        hash *= Prime; 

        hash ^= b & 255; 
        hash *= Prime; 
        hash ^= (b >> 8) & 255; 
        hash *= Prime; 
        hash ^= (b >> 16) & 255; 
        hash *= Prime; 
        hash ^= (b >> 24) & 255; 
        hash *= Prime; 
        hash ^= (b >> 32) & 255; 
        hash *= Prime; 
        hash ^= (b >> 40) & 255; 
        hash *= Prime; 
        hash ^= (b >> 48) & 255; 
        hash *= Prime; 
        hash ^= (b >> 56) & 255; 
        hash *= Prime; 

        return hash; 
    }
    
    public static ulong Compute(ulong a, ulong b, ulong c) {
        ulong hash = Offset; 
        
        hash ^= a & 255; 
        hash *= Prime; 
        hash ^= (a >> 8) & 255; 
        hash *= Prime; 
        hash ^= (a >> 16) & 255; 
        hash *= Prime; 
        hash ^= (a >> 24) & 255; 
        hash *= Prime; 
        hash ^= (a >> 32) & 255; 
        hash *= Prime; 
        hash ^= (a >> 40) & 255; 
        hash *= Prime; 
        hash ^= (a >> 48) & 255; 
        hash *= Prime; 
        hash ^= (a >> 56) & 255; 
        hash *= Prime; 

        hash ^= b & 255; 
        hash *= Prime; 
        hash ^= (b >> 8) & 255; 
        hash *= Prime; 
        hash ^= (b >> 16) & 255; 
        hash *= Prime; 
        hash ^= (b >> 24) & 255; 
        hash *= Prime; 
        hash ^= (b >> 32) & 255; 
        hash *= Prime; 
        hash ^= (b >> 40) & 255; 
        hash *= Prime; 
        hash ^= (b >> 48) & 255; 
        hash *= Prime; 
        hash ^= (b >> 56) & 255; 
        hash *= Prime; 

        hash ^= c & 255; 
        hash *= Prime; 
        hash ^= (c >> 8) & 255; 
        hash *= Prime; 
        hash ^= (c >> 16) & 255; 
        hash *= Prime; 
        hash ^= (c >> 24) & 255; 
        hash *= Prime; 
        hash ^= (c >> 32) & 255; 
        hash *= Prime; 
        hash ^= (c >> 40) & 255; 
        hash *= Prime; 
        hash ^= (c >> 48) & 255; 
        hash *= Prime; 
        hash ^= (c >> 56) & 255; 
        hash *= Prime; 

        return hash; 
    }
    
    public static ulong Compute(ulong a, ulong b, ulong c, ulong d) {
        ulong hash = Offset; 

        hash ^= a & 255; 
        hash *= Prime; 
        hash ^= (a >> 8) & 255; 
        hash *= Prime; 
        hash ^= (a >> 16) & 255; 
        hash *= Prime; 
        hash ^= (a >> 24) & 255; 
        hash *= Prime; 
        hash ^= (a >> 32) & 255; 
        hash *= Prime; 
        hash ^= (a >> 40) & 255; 
        hash *= Prime; 
        hash ^= (a >> 48) & 255; 
        hash *= Prime; 
        hash ^= (a >> 56) & 255; 
        hash *= Prime; 

        hash ^= b & 255; 
        hash *= Prime; 
        hash ^= (b >> 8) & 255; 
        hash *= Prime; 
        hash ^= (b >> 16) & 255; 
        hash *= Prime; 
        hash ^= (b >> 24) & 255; 
        hash *= Prime; 
        hash ^= (b >> 32) & 255; 
        hash *= Prime; 
        hash ^= (b >> 40) & 255; 
        hash *= Prime; 
        hash ^= (b >> 48) & 255; 
        hash *= Prime; 
        hash ^= (b >> 56) & 255; 
        hash *= Prime; 

        hash ^= c & 255; 
        hash *= Prime; 
        hash ^= (c >> 8) & 255; 
        hash *= Prime; 
        hash ^= (c >> 16) & 255; 
        hash *= Prime; 
        hash ^= (c >> 24) & 255; 
        hash *= Prime; 
        hash ^= (c >> 32) & 255; 
        hash *= Prime; 
        hash ^= (c >> 40) & 255; 
        hash *= Prime; 
        hash ^= (c >> 48) & 255; 
        hash *= Prime; 
        hash ^= (c >> 56) & 255; 
        hash *= Prime; 

        hash ^= d & 255; 
        hash *= Prime; 
        hash ^= (d >> 8) & 255; 
        hash *= Prime; 
        hash ^= (d >> 16) & 255; 
        hash *= Prime; 
        hash ^= (d >> 24) & 255; 
        hash *= Prime; 
        hash ^= (d >> 32) & 255; 
        hash *= Prime; 
        hash ^= (d >> 40) & 255; 
        hash *= Prime; 
        hash ^= (d >> 48) & 255; 
        hash *= Prime; 
        hash ^= (d >> 56) & 255; 
        hash *= Prime; 
        
        return hash; 
    }

}