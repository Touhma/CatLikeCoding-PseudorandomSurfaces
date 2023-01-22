﻿using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.LibNoises.Structs.Gradient
{
    public struct Simplex2D<G> : INoise where G : struct, IGradient
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sample4  GetNoise4(float4x3 positions, SmallXXHash4 hash, int frequency)
        {
            positions *= frequency * (1f / sqrt(3f));
            float4 skew = (positions.c0 + positions.c2) * ((sqrt(3f) - 1f) / 2f);
            float4 sx = positions.c0 + skew, sz = positions.c2 + skew;
            int4 x0 = (int4)floor(sx),
                x1 = x0 + 1,
                z0 = (int4)floor(sz),
                z1 = z0 + 1;

            bool4 xGz = sx - x0 > sz - z0;
            int4 xC = select(x0, x1, xGz), zC = select(z1, z0, xGz);

            SmallXXHash4
                h0 = hash.Eat(x0),
                h1 = hash.Eat(x1),
                hC = SmallXXHash4.Select(h0, h1, xGz);

            return default(G).EvaluateCombined(
                Kernel(h0.Eat(z0), x0, z0, positions) +
                Kernel(h1.Eat(z1), x1, z1, positions) +
                Kernel(hC.Eat(zC), xC, zC, positions)
            );
        }

        private static float4 Kernel(
            SmallXXHash4 hash, float4 lx, float4 lz, float4x3 positions
        )
        {
            float4 unskew = (lx + lz) * ((3f - sqrt(3f)) / 6f);
            float4 x = positions.c0 - lx + unskew, z = positions.c2 - lz + unskew;
            float4 f = 0.5f - x * x - z * z;
            f = f * f * f * 8f;
            ;
            return max(0f, f) * default(G).Evaluate(hash, x, z).v;
        }
    }
}