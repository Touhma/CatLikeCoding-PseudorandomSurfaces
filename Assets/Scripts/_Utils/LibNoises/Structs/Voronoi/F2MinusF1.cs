using _Utils.Interfaces;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct F2MinusF1 : IVoronoiFunction {

        public float4 Evaluate (float4x2 distances) => distances.c1 - distances.c0;
    }
}