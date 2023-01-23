using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Voronoi1D<L, D, F> : INoise
        where L : struct, ILattice
        where D : struct, IVoronoiDistance
        where F : struct, IVoronoiFunction {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sample4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            L l = default;
            D d = default;
            LatticeSpan4 x = l.GetLatticeSpan4(positions.c0, frequency);

            VoronoiData data = d.InitialData;
            for (int u = -1; u <= 1; u++) {
                SmallXXHash4 h = hash.Eat(l.ValidateSingleStep(x.p0 + u, frequency));
                data = d.UpdateVoronoiData(data, d.GetDistance(h.Floats01A + u - x.g0));
            }
            Sample4 s = default(F).Evaluate(d.Finalize1D(data));
            s.dx *= frequency;
            return s;
        }
    }
}