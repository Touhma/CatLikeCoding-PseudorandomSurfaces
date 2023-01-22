using _Utils;
using _Utils.Extensions;
using _Utils.Interfaces;
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

        public void Execute (int i) {
            float4x3 position = domainTRS.TransformVectors(transpose(positions[i]));
            SmallXXHash4 hash = SmallXXHash4.Seed(settings.seed);
            int frequency = settings.frequency;
            float amplitude = 1f, amplitudeSum = 0f;
            float4 sum = 0f;

            for (int o = 0; o < settings.octaves; o++) {
                sum += amplitude * default(N).GetNoise4(position, hash + o, frequency);
                amplitudeSum += amplitude;
                frequency *= settings.lacunarity;
                amplitude *= settings.persistence;
            }
            noise[i] = sum / amplitudeSum;
        }

        public static JobHandle ScheduleParallel (
            NativeArray<float3x4> positions, NativeArray<float4> noise,
            NoiseSettings settings, SpaceTRS domainTRS, int resolution, JobHandle dependency
        ) => new NoiseJob<N> {
            positions = positions,
            noise = noise,
            settings = settings,
            domainTRS = domainTRS.Matrix,
        }.ScheduleParallel(positions.Length, resolution, dependency);
    }
}