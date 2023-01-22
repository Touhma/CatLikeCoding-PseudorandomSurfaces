using _Utils.Interfaces;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct LatticeNormal : ILattice {

        public LatticeSpan4 GetLatticeSpan4 (float4 coordinates, int frequency) {
            coordinates *= frequency;
            float4 points = floor(coordinates);
            LatticeSpan4 span;
            span.p0 = (int4)points;
            span.p1 = span.p0 + 1;
            span.g0 = coordinates - span.p0;
            span.g1 = span.g0 - 1f;
            span.t = coordinates - points;
            span.t = span.t * span.t * span.t * (span.t * (span.t * 6f - 15f) + 10f);
            return span;
        }

        public int4 ValidateSingleStep (int4 points, int frequency) => points;
    }
}