using _Utils;
using _Utils.Extensions;
using _Utils.Interfaces;
using _Utils.LibMesh.Structs.MeshStreams;
using _Utils.NoisesLib;
using _Utils.NoisesLib.NoisesStructs;
using _Utils.NoisesLib.NoisesStructs.Commons;
using ProceduralMeshes.Commons;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
public struct SurfaceJob<N> : IJobFor where N : struct, INoise
{
    NativeArray<Vertex4> vertices;

    NoiseSettings settings;

    float3x4 domainTRS;

    float displacement;
    
    float3x3 derivativeMatrix;


    public void Execute (int i) {
        Vertex4 v = vertices[i];
        
        Sample4  noise = NoiseHelper.GetFractalNoise<N>(
            domainTRS.TransformVectors(transpose(float3x4(
                v.v0.position, v.v1.position, v.v2.position, v.v3.position
            ))),
            settings
        ) * displacement;

        v.v0.position.y = noise.v.x;
        v.v1.position.y = noise.v.y;
        v.v2.position.y = noise.v.z;
        v.v3.position.y = noise.v.w;
        
        float4x3 dNoise =
            derivativeMatrix.TransformVectors(noise.Derivatives);

        float4 normalizer = rsqrt(dNoise.c0 * dNoise.c0 + 1f);
        float4 tangent = dNoise.c0 * normalizer;
        
        v.v0.tangent = float4(normalizer.x, tangent.x, 0f, -1f);
        v.v1.tangent = float4(normalizer.y, tangent.y, 0f, -1f);
        v.v2.tangent = float4(normalizer.z, tangent.z, 0f, -1f);
        v.v3.tangent = float4(normalizer.w, tangent.w, 0f, -1f);
        
        normalizer = rsqrt(dNoise.c0 * dNoise.c0 + dNoise.c2 * dNoise.c2 + 1f);
        
        float4 normalX = -dNoise.c0 * normalizer;
        float4 normalZ = -dNoise.c2 * normalizer;
        
        v.v0.normal = float3(normalX.x, normalizer.x, normalZ.x);
        v.v1.normal = float3(normalX.y, normalizer.y, normalZ.y);
        v.v2.normal = float3(normalX.z, normalizer.z, normalZ.z);
        v.v3.normal = float3(normalX.w, normalizer.w, normalZ.w);
        
        vertices[i] = v;
    }

    public static JobHandle ScheduleParallel(
        Mesh.MeshData meshData, int resolution, NoiseSettings settings, SpaceTRS domain, float displacement, JobHandle dependency
    ) => new SurfaceJob<N>()
    {
        vertices = meshData.GetVertexData<Stream0>().Reinterpret<Vertex4>(12 * 4),
        settings = settings,
        domainTRS = domain.Matrix,
        derivativeMatrix = domain.DerivativeMatrix,
        displacement = displacement
    }.ScheduleParallel(meshData.vertexCount / 4, resolution, dependency);
}