using Unity.Mathematics;

namespace _Utils.Structs.NoisesStructs
{
    public struct LatticeSpan4 {
        public int4 p0, p1;
        public float4 g0, g1;
        public float4 t, dt;
    }
}