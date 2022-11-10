using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utils
{
    
    /// <summary>
    /// Helper class used to generate noise
    /// </summary>
    public static class NoiseHelper
    {
        private static readonly Dictionary<NoiseSignature, Vector2[]> GeneratedSeeds = new();

        public static Texture2D GenerateNoiseTexture(int width, int height, float[,] noiseMap)
        {
            var texture = new Texture2D(width, height);
            var colors = new Color[width * height];
            
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    colors[j * width + i] = Color.Lerp(Color.black, Color.white, noiseMap[i, j]);
                }
            }
            texture.SetPixels(colors);
            texture.Apply();
            
            return texture;
        }

        public static void SaveNoiseTextureToLocal(int width, int height, float[,] noiseMap, string name)
        {
            var bytes = GenerateNoiseTexture(width, height, noiseMap).EncodeToPNG();
            var dirPath = Application.dataPath + "/GeneratedNoises/";
            if(!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + name + ".png", bytes);
        }
        
        public static float[,] GenerateNoiseMap(int width, int height, float scale, int seed = 5237, int octave = 1, float persistence = 0.2f, float lacunarity = 1f, bool preserveOffsetRandomness = false)
        {
            return GenerateNoiseMap(width, height, scale, new Vector2(0, 0), seed, octave, persistence, lacunarity, preserveOffsetRandomness);
        }
        
        public static float[,] GenerateNoiseMap(int width, int height, float scale, Vector2 offset, int seed = 5237, int octave = 1, float persistence = 0.2f, float lacunarity = 1f, bool preserveOffsetRandomness = false)
        {
            var map = new float[width,height];

            var minNoiseHeight = float.MaxValue;
            var maxNoiseHeight = float.MinValue;
            var halfWidth = width / 2;
            var halfHeight = height / 2;
            
            var randOffset = preserveOffsetRandomness ? GenerateSeeds(seed, octave) : GetGeneratedSeeds(seed, octave);

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            persistence = Mathf.Clamp(persistence, 0, 1);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var amplitude = 1f;
                    var frequency = 1f;
                    var noiseHeight = 0f;

                    for (var i = 0; i < octave; i++)
                    {
                        var x2 = (x - halfWidth) / scale * frequency + randOffset[i].x;
                        var y2 = (y - halfHeight) / scale * frequency + randOffset[i].y;
                        var value = Mathf.PerlinNoise(x2 + offset.x, y2 + offset.y) * 2 - 1;
                        
                        noiseHeight += value * amplitude;
                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    } 
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }
                    
                    map[x, y] = noiseHeight;
                }
            }

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    map[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, y]);
                }
            }
            
            return map;
        }

        private static Vector2[] GetGeneratedSeeds(int seed, int octaves)
        {
            var noiseSignature = new NoiseSignature(seed, octaves);
            if (GeneratedSeeds.ContainsKey(noiseSignature))
            {
                return GeneratedSeeds[noiseSignature];
            }

            var seeds = GenerateSeeds(seed, octaves);
            GeneratedSeeds.Add(noiseSignature, seeds);
            return seeds;
        }
        
        private static Vector2[] GenerateSeeds(int seed, int octaves)
        {
            var rand = new System.Random(seed);
            var res = new Vector2[octaves];
            for (var i = 0; i < octaves; i++)
            {
                var offX = rand.Next(-100000, 100000);
                var offY = rand.Next(-100000, 100000);

                res[i] = new Vector2(offX, offY);
            }

            return res;
        }

        private readonly struct NoiseSignature
        {
            public readonly int Seed;
            public readonly int Octave;

            public NoiseSignature(int seed, int octave)
            {
                Seed = seed;
                Octave = octave;
            }

            public static bool operator ==(NoiseSignature a, NoiseSignature b)
            {
                return a.Seed == b.Seed && a.Octave == b.Octave;
            }

            public static bool operator !=(NoiseSignature a, NoiseSignature b)
            {
                return !(a == b);
            }
            
            public bool Equals(NoiseSignature other)
            {
                return Seed == other.Seed && Octave == other.Octave;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Seed, Octave);
            }
            
            public override bool Equals(object obj)
            {
                return obj is NoiseSignature other && Equals(other);
            }
        }
    }
}