using _Utils;
using _Utils.Extensions;
using _Utils.Interfaces;
using _Utils.NoisesLib;
using _Utils.NoisesLib.NoisesStructs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;


namespace Jobs
{

    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct NoiseJob<N> : IJobFor where N : struct, INoise {

        [ReadOnly]
        public NativeArray<float3x4> positions;

        [WriteOnly]
        public NativeArray<float4> noise;

        public NoiseSettings settings;

        public float3x4 domainTRS;

        public void Execute (int i) => noise[i] = NoiseHelper.GetFractalNoise<N>(
            domainTRS.TransformVectors(transpose(positions[i])), settings
        ).v;

        public static JobHandle ScheduleParallel (
            NativeArray<float3x4> positions, NativeArray<float4> noise,
            NoiseSettings settings, SpaceTRS domainTRS, int resolution, JobHandle dependency
        ) => new NoiseJob<N> {
            positions = positions,
            noise = noise,
            settings = settings,
            domainTRS = domainTRS.Matrix,
        }.ScheduleParallel(positions.Length, resolution, dependency);
        
        public delegate JobHandle ScheduleDelegate (NativeArray<float3x4> positions, NativeArray<float4> noise, NoiseSettings settings, SpaceTRS domainTRS, int resolution, JobHandle dependency);
    }
}