using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Voronoi1D<L, D, F> : INoise
        where L : struct, ILattice
        where D : struct, IVoronoiDistance
        where F : struct, IVoronoiFunction {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            L l = default;
            D d = default;
            LatticeSpan4 x = l.GetLatticeSpan4(positions.c0, frequency);

            float4x2 minima = 2f;
            for (int u = -1; u <= 1; u++) {
                SmallXXHash4 h = hash.Eat(l.ValidateSingleStep(x.p0 + u, frequency));
                minima =
                    VoronoiHelper.UpdateVoronoiMinima(minima, d.GetDistance(h.Floats01A + u - x.g0));
            }
            return default(F).Evaluate(d.Finalize1D(minima));
        }
    }
}