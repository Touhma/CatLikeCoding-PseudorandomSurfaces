using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace ProceduralMeshes.Commons
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Stream0 {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }
}