using _Utils.Interfaces;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct NoiseTurbulence<G> : IGradient where G : struct, IGradient {

        public float4 Evaluate (SmallXXHash4 hash, float4 x) => default(G).Evaluate(hash, x);

        public float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y) => default(G).Evaluate(hash, x, y);

        public float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z) => default(G).Evaluate(hash, x, y, z);

        public float4 EvaluateCombined (float4 value) => abs(default(G).EvaluateCombined(value));
    }
}