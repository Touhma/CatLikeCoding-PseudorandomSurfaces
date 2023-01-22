using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.LibNoises.Structs.Gradient
{
    public struct Simplex1D<G> : INoise where G : struct, IGradient {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sample4  GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            positions *= frequency;
            int4 x0 = (int4)floor(positions.c0), x1 = x0 + 1;;
            Sample4 s = default(G).EvaluateCombined(
                Kernel(hash.Eat(x0), x0, positions) + Kernel(hash.Eat(x1), x1, positions)
            );
            s.dx *= frequency;
            return s;
        }

        private static Sample4 Kernel (SmallXXHash4 hash, float4 lx, float4x3 positions) {
            float4 x = positions.c0 - lx;
            float4 f = 1f - x * x;
            Sample4  g = default(G).Evaluate(hash, x);
            return new Sample4 {
                v = f * f * f * g.v,
                dx = f * f * -6f * x * g.v
            };
        }
    }
}