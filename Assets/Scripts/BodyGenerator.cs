using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBodyGenerator 
{
    Material Material { get; } 

    float BaseHeight { get; } 

    bool IsPlanet { get; } 

    float GetHeight(Vector3 position, int lod); 
}

public class DefaultGenerator : IBodyGenerator  
{
    private Material material; 

    public Material Material { get { return material; } } 

    public float BaseHeight { get { return 1.0f; } } 

    public bool IsPlanet { get { return false; } } 

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

    public PlanetGenerator() 
    {
        if (material == null) material = Resources.Load("Materials/PlanetBaseMaterial") as Material; 
    }

    private float Fractal(Vector3 pos, int lod) 
    {
        float freq = 2.0f; 
        float amp = 1.0f; 
        float sum = 0.0f; 
        float total = 0.0f; 

        for (uint i = 0; i < 4 + lod * 2; i++) 
        {
            total += amp; 
            sum += amp * (float) Noise.GetNoise3D(pos.x * freq, pos.y * freq, pos.z * freq, i + 10); 

            freq *= 2; 
            amp *= 0.55f; 
        }

        return sum / total; 
    }

    public float GetHeight(Vector3 position, int lod) 
    {
        return 1 + MathUtil.Clamp(0, 1, 0.2f * Fractal(position, lod)); 
    }
}

public class StarGenerator : IBodyGenerator  
{
    private Material material; 

    public Material Material { get{ return material; } } 

    public float BaseHeight { get { return 10.0f; } } 

    public bool IsPlanet { get { return false; } } 

    public StarGenerator() 
    {
        material = Resources.Load("Materials/StarBaseMaterial") as Material; 
    }

    public float GetHeight(Vector3 position, int lod) 
    {
        return 10; 
    }
}