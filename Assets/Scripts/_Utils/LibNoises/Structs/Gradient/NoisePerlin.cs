using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct NoisePerlin: IGradient {

        public Sample4  Evaluate (SmallXXHash4 hash, float4 x) => GradientHelper.Line(hash, x);

        public Sample4  Evaluate (SmallXXHash4 hash, float4 x, float4 y) => GradientHelper.Square(hash, x, y) * (2f / 0.53528f);

        public Sample4  Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z) => GradientHelper.Octahedron(hash, x, y, z) * (1f / 0.56290f);

        public Sample4 EvaluateCombined (Sample4 value) => value;
    }
}