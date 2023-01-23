using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

namespace _Utils.NoisesLib
{
    public static class NoiseHelper
    {
        public static Sample4 GetFractalNoise<N>(float4x3 position, NoiseSettings settings) where N : struct, INoise
        {
            SmallXXHash4 hash = SmallXXHash4.Seed(settings.seed);
            int frequency = settings.frequency;
            float amplitude = 1f, amplitudeSum = 0f;
            Sample4 sum = default;

            for (int o = 0; o < settings.octaves; o++)
            {
                sum += amplitude * default(N).GetNoise4(position, hash + o, frequency);
                amplitudeSum += amplitude;
                frequency *= settings.lacunarity;
                amplitude *= settings.persistence;
            }

            return sum / amplitudeSum;
        }
    }
}