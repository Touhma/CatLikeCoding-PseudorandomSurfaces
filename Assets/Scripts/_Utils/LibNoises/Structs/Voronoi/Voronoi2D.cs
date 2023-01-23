using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Voronoi2D<L, D, F> : INoise
        where L : struct, ILattice
        where D : struct, IVoronoiDistance
        where F : struct, IVoronoiFunction {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sample4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            L l = default;
            D d = default;
            LatticeSpan4
                x = l.GetLatticeSpan4(positions.c0, frequency),
                z = l.GetLatticeSpan4(positions.c2, frequency);

            VoronoiData data = d.InitialData;
            for (int u = -1; u <= 1; u++) {
                SmallXXHash4 hx = hash.Eat(l.ValidateSingleStep(x.p0 + u, frequency));
                float4 xOffset = u - x.g0;
                for (int v = -1; v <= 1; v++) {
                    SmallXXHash4 h = hx.Eat(l.ValidateSingleStep(z.p0 + v, frequency));
                    float4 zOffset = v - z.g0;
                    data = d.UpdateVoronoiData(data, d.GetDistance(h.Floats01A + xOffset, h.Floats01B + zOffset));
                    data = d.UpdateVoronoiData(data, d.GetDistance(h.Floats01C + xOffset, h.Floats01D + zOffset));
                }
            }
            Sample4 s = default(F).Evaluate(d.Finalize2D(data));
            s.dx *= frequency;
            s.dz *= frequency;
            return s;
        }
    }
}