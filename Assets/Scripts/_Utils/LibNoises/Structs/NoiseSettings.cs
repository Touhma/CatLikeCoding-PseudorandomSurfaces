using System;
using UnityEngine;

namespace _Utils.NoisesLib.NoisesStructs
{
    [Serializable]
    public struct NoiseSettings {

        public int seed;
        
        [Min(1)]
        public int frequency;
        
        [Range(1, 6)]
        public int octaves;
        
        [Range(2, 4)]
        public int lacunarity;
        
        [Range(0f, 1f)]
        public float persistence;

        public static NoiseSettings Default => new (){
            frequency = 4,
            octaves = 1,
            lacunarity = 2,
            persistence = 0.5f
        };
    }
}