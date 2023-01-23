using _Utils;
using _Utils.NoisesLib.NoisesStructs;
using Unity.Jobs;
using UnityEngine;

namespace Jobs.Delegate
{
    public delegate JobHandle SurfaceJobScheduleDelegate (
        Mesh.MeshData meshData, 
        int resolution, 
        NoiseSettings settings, 
        SpaceTRS domain,
        float displacement, 
        bool isPlane, 
        int sphereRadius,
        JobHandle dependency
    );
}