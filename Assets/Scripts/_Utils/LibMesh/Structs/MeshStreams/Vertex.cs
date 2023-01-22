using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace ProceduralMeshes {

	[StructLayout(LayoutKind.Sequential)]
	public struct Vertex {
		public float3 position, normal;
		public float4 tangent;
		public float2 texCoord0;
	}
}