using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Lattice1D<L, G> : INoise where L : struct, ILattice where G : struct, IGradient {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float4 GetNoise4(float4x3 positions, SmallXXHash4 hash, int frequency) {
            LatticeSpan4 x = default(L).GetLatticeSpan4(positions.c0, frequency);

            G g = default;
            return g.EvaluateCombined(lerp(
                g.Evaluate(hash.Eat(x.p0), x.g0), g.Evaluate(hash.Eat(x.p1), x.g1), x.t
            ));
        }
    }
}