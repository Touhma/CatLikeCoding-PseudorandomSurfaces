using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Lattice2D<L, G> : INoise where L : struct, ILattice where G : struct, IGradient {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            L l = default;
            LatticeSpan4
                x = l.GetLatticeSpan4(positions.c0, frequency),
                z = l.GetLatticeSpan4(positions.c2, frequency);

            SmallXXHash4 h0 = hash.Eat(x.p0), h1 = hash.Eat(x.p1);

            G g = default;
            return g.EvaluateCombined(lerp(
                lerp(
                    g.Evaluate(h0.Eat(z.p0), x.g0, z.g0),
                    g.Evaluate(h0.Eat(z.p1), x.g0, z.g1),
                    z.t
                ),
                lerp(
                    g.Evaluate(h1.Eat(z.p0), x.g1, z.g0),
                    g.Evaluate(h1.Eat(z.p1), x.g1, z.g1),
                    z.t
                ),
                x.t
            ));
        }
    }
}