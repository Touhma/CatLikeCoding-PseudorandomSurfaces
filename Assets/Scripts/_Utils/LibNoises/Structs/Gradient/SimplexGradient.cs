using _Utils.Interfaces;
using _Utils.NoisesLib;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.LibNoises.Structs.Gradient
{
    public struct SimplexGradient: IGradient
    {
        public Sample4 Evaluate (SmallXXHash4 hash, float4 x) => GradientHelper.Line(hash, x) * (32f / 27f);

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y) => GradientHelper.Circle(hash, x, y) * (5.832f / sqrt(2f));

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z) => GradientHelper.Sphere(hash, x, y, z) * (1024f / (125f * sqrt(3f)));

        public Sample4 EvaluateCombined (Sample4 value) => value;
    }
}