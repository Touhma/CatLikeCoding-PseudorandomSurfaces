using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Lattice3D<L, G> : INoise
        where L : struct, ILattice where G : struct, IGradient {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sample4  GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            L l = default;
            LatticeSpan4
                x = l.GetLatticeSpan4(positions.c0, frequency),
                y = l.GetLatticeSpan4(positions.c1, frequency),
                z = l.GetLatticeSpan4(positions.c2, frequency);

            SmallXXHash4
                h0 = hash.Eat(x.p0), h1 = hash.Eat(x.p1),
                h00 = h0.Eat(y.p0), h01 = h0.Eat(y.p1),
                h10 = h1.Eat(y.p0), h11 = h1.Eat(y.p1);

            G g = default;
            return g.EvaluateCombined(lerp(
                lerp(
                    lerp(
                        g.Evaluate(h00.Eat(z.p0), x.g0, y.g0, z.g0).v,
                        g.Evaluate(h00.Eat(z.p1), x.g0, y.g0, z.g1).v,
                        z.t
                    ),
                    lerp(
                        g.Evaluate(h01.Eat(z.p0), x.g0, y.g1, z.g0).v,
                        g.Evaluate(h01.Eat(z.p1), x.g0, y.g1, z.g1).v,
                        z.t
                    ),
                    y.t
                ),
                lerp(
                    lerp(
                        g.Evaluate(h10.Eat(z.p0), x.g1, y.g0, z.g0).v,
                        g.Evaluate(h10.Eat(z.p1), x.g1, y.g0, z.g1).v,
                        z.t
                    ),
                    lerp(
                        g.Evaluate(h11.Eat(z.p0), x.g1, y.g1, z.g0).v,
                        g.Evaluate(h11.Eat(z.p1), x.g1, y.g1, z.g1).v,
                        z.t
                    ),
                    y.t
                ),
                x.t
            ));
        }
    }
}