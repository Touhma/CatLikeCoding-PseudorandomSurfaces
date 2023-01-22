using _Utils.Interfaces;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct F1 : IVoronoiFunction {

        public float4 Evaluate (float4x2 distances) => distances.c0;
    }
}