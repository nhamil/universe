using System; 
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics; 
using UnityEngine;

public interface IBodyGenerator 
{
    Material Material { get; } 

    float BaseHeight { get; } 

    bool IsPlanet { get; } 
    bool IsMoon { get; } 

    Color WaterColor { get; }
    Color LowLandColor { get; } 
    Color HighLandColor { get; } 

    float GetHeight(Vector3 position, int lod); 
}

public class DefaultGenerator : IBodyGenerator  
{
    private Material material; 

    public Material Material { get { return material; } } 

    public float BaseHeight { get { return 1.0f; } } 

    public bool IsPlanet { get { return false; } } 
    public bool IsMoon { get { return false; } } 

    public Color WaterColor { get; private set; }
    public Color LowLandColor { get; private set; } 
    public Color HighLandColor { get; private set; } 

    public DefaultGenerator() 
    {
        material = new Material(Shader.Find("Standard"));  
    }

    public float GetHeight(Vector3 position, int lod) 
    {
        return position.magnitude; 
    }
}

public class PlanetGenerator : IBodyGenerator  
{
    private static Material material; 
    
    public Material Material { get{ return material; } } 

    public float BaseHeight { get; private set; } 
    public float WaterBias { get; private set; } 

    public float BaseFreq { get; private set; } 
    public float Persistance { get; private set; } 
    public float HeightMul { get; private set; } 

    public bool IsPlanet { get { return true; } } 
    public bool IsMoon { get; private set; } 

    public ulong Seed { get; private set; } 

    public Color WaterColor { get; private set; }
    public Color LowLandColor { get; private set; } 
    public Color HighLandColor { get; private set; } 

    public Color AtmosColor { get; private set; } 
    public float AtmosStrength { get; private set; } 

    public PlanetGenerator(float bh, float v) : this(bh, v, false) {}

    public PlanetGenerator(float baseHeight, float variance, bool moon) 
    {
        if (material == null) material = Resources.Load("Materials/PlanetBaseMaterial") as Material; 
        IsMoon = moon; 
        HeightMul = moon ? 0.2f : 1; 
        Seed = (ulong) NanoTime(); 
        System.Random rand = new System.Random((int) Seed); 
        BaseHeight = (float) (rand.NextDouble() * variance + baseHeight - variance/2); 
        WaterBias = (float) (rand.NextDouble() * 0.04 - 0.02); 
        BaseFreq = (float) (rand.NextDouble() * 1.0 + 0.5); 
        Persistance = (float) (rand.NextDouble() * 0.3 + 0.45); 
        if (rand.NextDouble() < 0.5) // earth 
        {
            WaterColor = new Color(0.0f, 0.1f, 0.5f); 
            LowLandColor = new Color(0.1f, 0.5f, 0.1f); 
            HighLandColor = new Color(0.2f, 0.7f, 0.2f); 
            AtmosColor = new Color(0.2f, 0.3f, 1.0f); 
            AtmosStrength = 0.5f; 
        }
        else // alien world 
        {
            WaterColor = StrongColor(rand); 
            LowLandColor = StrongColor(rand); 
            HighLandColor = StrongColor(rand); 
            AtmosColor = StrongColor(rand); 
            AtmosStrength = (float) (rand.NextDouble() * 1.0); 
        }
        if (moon) 
        {
            WaterBias += 0.02f;
            WaterColor = StrongColor(rand); 
            LowLandColor = StrongColor(rand); 
            HighLandColor = StrongColor(rand); 
        }
        // else // alien world 
        // {
        //     WaterColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble()); 
        //     LowLandColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
        //     HighLandColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
        //     AtmosColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
        //     AtmosStrength = (float) rand.NextDouble(); 
        // }
    }

    private static Color StrongColor(System.Random rand) 
    {
        Vector3 v = new Vector3((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble()); 
        v = v.normalized; 
        v *= (float) (rand.NextDouble() * 0.3 + 0.7); 
        return new Color(v.x, v.y, v.z); 
    }

    // https://stackoverflow.com/questions/1551742/what-is-the-equivalent-to-system-nanotime-in-net
    private static long NanoTime() 
    {
        long nano = 10000L * Stopwatch.GetTimestamp();
        nano /= TimeSpan.TicksPerMillisecond;
        nano *= 100L;
        return nano;
    }

    private float Fractal(Vector3 pos, int lod) 
    {
        float freq = 2.0f; 
        float amp = 1.0f; 
        float sum = 0.0f; 
        float total = 0.0f; 

        for (uint i = 0; i < 3 + lod * 1.5; i++) 
        {
            total += amp; 
            sum += amp * ((float) Noise.GetNoise3D(pos.x * freq * BaseFreq, pos.y * freq * BaseFreq, pos.z * freq * BaseFreq, Seed + i + 10)); 

            freq *= 2; 
            amp *= Persistance; 
        }

        return sum / total; 
    }

    public float GetHeight(Vector3 position, int lod) 
    {
        return BaseHeight + MathUtil.Clamp(0, 1, 0.03f * Fractal(position, lod) + WaterBias) * HeightMul; 
    }
}

public class StarGenerator : IBodyGenerator  
{
    private static Color StarColor = new Color(1, 1, 0); 

    private Material material; 

    public Material Material { get{ return material; } } 

    public float BaseHeight { get { return 10.0f; } } 

    public bool IsPlanet { get { return false; } } 
    public bool IsMoon { get { return false; } } 

    public Color WaterColor { get { return StarColor; } }
    public Color LowLandColor { get { return StarColor; } }
    public Color HighLandColor { get { return StarColor; } }

    public StarGenerator() 
    {
        material = Resources.Load("Materials/StarBaseMaterial") as Material; 
    }

    public float GetHeight(Vector3 position, int lod) 
    {
        return 10; 
    }
}