using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct NoiseTurbulence<G> : IGradient where G : struct, IGradient {

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x) => default(G).Evaluate(hash, x);

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y) => default(G).Evaluate(hash, x, y);

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z) => default(G).Evaluate(hash, x, y, z);

        public Sample4 EvaluateCombined (Sample4 value) {
            Sample4 s = default(G).EvaluateCombined(value);
            s.dx = select(-s.dx, s.dx, s.v >= 0f);
            s.dy = select(-s.dy, s.dy, s.v >= 0f);
            s.dz = select(-s.dz, s.dz, s.v >= 0f);
            s.v = abs(s.v);
            return s;
        }
    }
}