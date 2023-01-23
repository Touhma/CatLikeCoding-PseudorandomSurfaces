﻿using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.LibNoises.Structs.Gradient
{
    public struct Simplex3D<G> : INoise where G : struct, IGradient
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sample4  GetNoise4(float4x3 positions, SmallXXHash4 hash, int frequency)
        {
            positions *= frequency * 0.6f;
            float4 skew = (positions.c0 + positions.c1 + positions.c2) * (1f / 3f);
            float4
                sx = positions.c0 + skew,
                sy = positions.c1 + skew,
                sz = positions.c2 + skew;
            int4
                x0 = (int4)floor(sx),
                x1 = x0 + 1,
                y0 = (int4)floor(sy),
                y1 = y0 + 1,
                z0 = (int4)floor(sz),
                z1 = z0 + 1;

            bool4
                xGy = sx - x0 > sy - y0,
                xGz = sx - x0 > sz - z0,
                yGz = sy - y0 > sz - z0;

            bool4
                xA = xGy & xGz,
                xB = xGy | (xGz & yGz),
                yA = !xGy & yGz,
                yB = !xGy | (xGz & yGz),
                zA = (xGy & !xGz) | (!xGy & !yGz),
                zB = !(xGz & yGz);

            int4
                xCA = select(x0, x1, xA),
                xCB = select(x0, x1, xB),
                yCA = select(y0, y1, yA),
                yCB = select(y0, y1, yB),
                zCA = select(z0, z1, zA),
                zCB = select(z0, z1, zB);

            SmallXXHash4
                h0 = hash.Eat(x0),
                h1 = hash.Eat(x1),
                hA = SmallXXHash4.Select(h0, h1, xA),
                hB = SmallXXHash4.Select(h0, h1, xB);

            Sample4 s = default(G).EvaluateCombined(
                Kernel(h0.Eat(y0).Eat(z0), x0, y0, z0, positions) +
                Kernel(h1.Eat(y1).Eat(z1), x1, y1, z1, positions) +
                Kernel(hA.Eat(yCA).Eat(zCA), xCA, yCA, zCA, positions) +
                Kernel(hB.Eat(yCB).Eat(zCB), xCB, yCB, zCB, positions)
            );
            s.dx *= frequency * 0.6f;
            s.dy *= frequency * 0.6f;
            s.dz *= frequency * 0.6f;
            return s;
        }

        private Sample4 Kernel(
            SmallXXHash4 hash, float4 lx, float4 ly, float4 lz, float4x3 positions
        )
        {
            float4 unskew = (lx + ly + lz) * (1f / 6f);
            float4
                x = positions.c0 - lx + unskew,
                y = positions.c1 - ly + unskew,
                z = positions.c2 - lz + unskew;
            float4 f = 0.5f - x * x - y * y - z * z;
            Sample4 g = default(G).Evaluate(hash, x, y, z);
            return new Sample4 {
                v = f * g.v,
                dx = f * g.dx - 6f * x * g.v,
                dy = f * g.dy - 6f * y * g.v,
                dz = f * g.dz - 6f * z * g.v
            } * f * f * select(0f, 8f, f >= 0f);
        }
    }
}