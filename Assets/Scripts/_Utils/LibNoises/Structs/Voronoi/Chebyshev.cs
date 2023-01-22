using _Utils.Interfaces;
using Unity.Mathematics;
using static Unity.Mathematics.math;
namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Chebyshev : IVoronoiDistance {

        public float4 GetDistance (float4 x) => abs(x);

        public float4 GetDistance (float4 x, float4 y) => max(abs(x), abs(y));

        public float4 GetDistance (float4 x, float4 y, float4 z) => max(max(abs(x), abs(y)), abs(z));

        public float4x2 Finalize1D (float4x2 minima) => minima;

        public float4x2 Finalize2D (float4x2 minima) => minima;

        public float4x2 Finalize3D (float4x2 minima) => minima;
    }
}