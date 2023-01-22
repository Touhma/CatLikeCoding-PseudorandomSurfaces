using _Utils.Interfaces;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct NoisePerlin: IGradient {

        public float4 Evaluate (SmallXXHash4 hash, float4 x) => GradientHelper.Line(hash, x);

        public float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y) => GradientHelper.Square(hash, x, y) * (2f / 0.53528f);

        public float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z) => GradientHelper.Octahedron(hash, x, y, z) * (1f / 0.56290f);

        public float4 EvaluateCombined (float4 value) => value;
    }
}