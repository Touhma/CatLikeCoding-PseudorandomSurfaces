using _Utils.Interfaces;
using Unity.Mathematics;

namespace _Utils.Structs.NoiseStruct
{
    public struct NoiseGradient: IGradient
    {
        public float4 Evaluate (SmallXXHash4 hash, float4 x) => hash.Floats01A * 2f - 1f;

        public float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y) => hash.Floats01A * 2f - 1f;

        public float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z) => hash.Floats01A * 2f - 1f;
        
        public float4 EvaluateCombined (float4 value) => value;
    }
}