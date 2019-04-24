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

    public float BaseHeight { get { return 1.0f; } } 

    public bool IsPlanet { get { return true; } } 

    public ulong Seed { get; private set; } 

    public Color WaterColor { get; private set; }
    public Color LowLandColor { get; private set; } 
    public Color HighLandColor { get; private set; } 

    public Color AtmosColor { get; private set; } 
    public float AtmosStrength { get; private set; } 

    public PlanetGenerator() 
    {
        if (material == null) material = Resources.Load("Materials/PlanetBaseMaterial") as Material; 
        Seed = (ulong) NanoTime(); 
        System.Random rand = new System.Random((int) Seed); 
        if (rand.NextDouble() < 0.5) // earth 
        {
            WaterColor = new Color(0.0f, 0.1f, 0.3f); 
            LowLandColor = new Color(0.1f, 0.2f, 0.1f); 
            HighLandColor = new Color(0.2f, 0.7f, 0.2f); 
            AtmosColor = new Color(0.3f, 0.5f, 0.9f); 
            AtmosStrength = 0.5f; 
        }
        else // alien world 
        {
            WaterColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble()); 
            LowLandColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
            HighLandColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
            AtmosColor = new Color((float) rand.NextDouble(), (float) rand.NextDouble(), (float) rand.NextDouble());
            AtmosStrength = (float) rand.NextDouble(); 
        }
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
            sum += amp * ((float) Noise.GetNoise3D(pos.x * freq, pos.y * freq, pos.z * freq, Seed + i + 10)); 

            freq *= 2; 
            amp *= 0.55f; 
        }

        return sum / total; 
    }

    public float GetHeight(Vector3 position, int lod) 
    {
        return 1 + MathUtil.Clamp(0, 1, 0.05f * Fractal(position, lod)); 
    }
}

public class StarGenerator : IBodyGenerator  
{
    private static Color StarColor = new Color(1, 1, 0); 

    private Material material; 

    public Material Material { get{ return material; } } 

    public float BaseHeight { get { return 10.0f; } } 

    public bool IsPlanet { get { return false; } } 

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