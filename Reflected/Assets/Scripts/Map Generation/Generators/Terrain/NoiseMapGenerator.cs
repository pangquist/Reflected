using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public float seed;
    public float frequency;
    public float amplitude;
}
public class NoiseMapGenerator : MonoBehaviour
{
    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ, Wave[] waves, float randomSeed)
    {
        // create an empty noise map with the mapDepth and mapWidth coordinates
        float[,] noiseMap = new float[mapDepth, mapWidth];
        for (int zIndex = 0; zIndex < mapDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                // calculate sample indices based on the coordinates and the scale
                float sampleX = (xIndex + offsetX) / scale;
                float sampleZ = (zIndex + offsetZ) / scale;
                // generate noise value using PerlinNoise
                float noise = 0f;
                float normalization = 0f;
                foreach(Wave wave in waves)
                {
                    noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed + randomSeed, sampleZ * wave.frequency + wave.seed + randomSeed);
                    //noise += wave.amplitude * perlin(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }

                noise /= normalization;
                noiseMap[zIndex, xIndex] = noise;
            }
        }
        return noiseMap;
    }

    float interpolate(float a0, float a1, float w)
    {
        if (0.0 > w) return a0;
        if (1.0 < w) return a1;

        return (a1 - a0) * w + a0;
    }

    Vector2 randomGradient(int? ix, int? iy)
    {
        // No precomputed gradients mean this works for any number of grid coordinates
        const int w = 8 * sizeof(int);
        const int s = w / 2; // rotation width
        int? a = ix, b = iy;
        a *= -328415744;
        b ^= a << s | a >> w - s;
        b *= 1911520717;
        a ^= (b << s) | b >> w - s;
        a *= 2048419325;
        float random = (float)(a * (3.14159265f / ~(~0u >> 1))); // in [0, 2*Pi]
        Vector2 v;
        v.x = Mathf.Cos(random); v.y = Mathf.Sin(random);
        return v;
    }

    // Computes the dot product of the distance and gradient vectors.
    float dotGridGradient(int ix, int iy, float x, float y)
    {
        // Get gradient from integer coordinates
        Vector2 gradient = randomGradient(ix, iy);

        // Compute the distance vector
        float dx = x - (float)ix;
        float dy = y - (float)iy;

        // Compute the dot-product
        return (dx * gradient.x + dy * gradient.y);
    }

    float perlin(float x, float y)
    {
        // Determine grid cell coordinates
        int x0 = (int)Mathf.Floor(x);
        int x1 = x0 + 1;
        int y0 = (int)Mathf.Floor(y);
        int y1 = y0 + 1;

        // Determine interpolation weights
        float sx = x - (float)x0;
        float sy = y - (float)y0;

        // Interpolate between grid point gradients
        float n0, n1, ix0, ix1, value;

        n0 = dotGridGradient(x0, y0, x, y);
        n1 = dotGridGradient(x1, y0, x, y);
        ix0 = interpolate(n0, n1, sx);

        n0 = dotGridGradient(x0, y1, x, y);
        n1 = dotGridGradient(x1, y1, x, y);
        ix1 = interpolate(n0, n1, sx);

        value = interpolate(ix0, ix1, sy);
        return value * 0.5f + 0.4f;
    }
}
