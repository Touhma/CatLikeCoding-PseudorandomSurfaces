using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Lattice2D<L, G> : INoise
        where L : struct, ILattice where G : struct, IGradient {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Sample4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
            L l = default;
            LatticeSpan4
                x = l.GetLatticeSpan4(positions.c0, frequency),
                z = l.GetLatticeSpan4(positions.c2, frequency);

            SmallXXHash4 h0 = hash.Eat(x.p0), h1 = hash.Eat(x.p1);

            G g = default;
            Sample4
                a = g.Evaluate(h0.Eat(z.p0), x.g0, z.g0),
                b = g.Evaluate(h0.Eat(z.p1), x.g0, z.g1),
                c = g.Evaluate(h1.Eat(z.p0), x.g1, z.g0),
                d = g.Evaluate(h1.Eat(z.p1), x.g1, z.g1);

            return g.EvaluateCombined(new Sample4 {
                v = lerp(lerp(a.v, b.v, z.t), lerp(c.v, d.v, z.t), x.t),
                dx = frequency * (
                    lerp(lerp(a.dx, b.dx, z.t), lerp(c.dx, d.dx, z.t), x.t) +
                    (lerp(c.v, d.v, z.t) - lerp(a.v, b.v, z.t)) * x.dt
                ),
                dz = frequency * lerp(
                    lerp(a.dz, b.dz, z.t) + (b.v - a.v) * z.dt,
                    lerp(c.dz, d.dz, z.t) + (d.v - c.v) * z.dt,
                    x.t
                )
            });
        }
    }
}