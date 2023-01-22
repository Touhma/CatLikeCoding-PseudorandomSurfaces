using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using Unity.Burst;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.LibNoises.Structs.Gradient
{

    public struct Simplex1D<G> : INoise where G : struct, IGradient {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            positions *= frequency;
            int4 x0 = (int4)floor(positions.c0), x1 = x0 + 1;;
            return Kernel(hash.Eat(x0), x0, positions) + Kernel(hash.Eat(x1), x1, positions);
        }

        private static float4 Kernel (SmallXXHash4 hash, float4 lx, float4x3 positions) {
            float4 x = positions.c0 - lx;
            float4 f = 1f - x * x;
            f = f * f * f;
            return f * default(G).Evaluate(hash, x);
        }
    }
}