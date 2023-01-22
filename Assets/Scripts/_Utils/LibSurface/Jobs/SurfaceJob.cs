using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

using static Unity.Mathematics.math;

[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
public struct SurfaceJob : IJobFor {

	NativeArray<float3> positions;

	public void Execute (int i) {
		float3 p = positions[i];
		p.y = abs(p.x);
		positions[i] = p;
	}
}