using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace ProceduralMeshes.Streams {

	[StructLayout(LayoutKind.Sequential)]
	public struct TriangleUInt32 {

		public uint a, b, c;

		public static implicit operator TriangleUInt32 (int3 t) => new TriangleUInt32 {
			a = (uint)t.x,
			b = (uint)t.y,
			c = (uint)t.z
		};
	}
}