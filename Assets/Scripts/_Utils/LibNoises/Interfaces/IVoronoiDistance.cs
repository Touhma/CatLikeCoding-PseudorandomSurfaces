using _Utils.NoisesLib.NoisesStructs;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

namespace _Utils.Interfaces
{
    public interface IVoronoiDistance
    {
        Sample4 GetDistance(float4 x);

        Sample4 GetDistance(float4 x, float4 y);

        Sample4 GetDistance(float4 x, float4 y, float4 z);

        VoronoiData Finalize1D(VoronoiData data);

        VoronoiData Finalize2D(VoronoiData data);

        VoronoiData Finalize3D(VoronoiData data);
        
        VoronoiData UpdateVoronoiData (VoronoiData data, Sample4 sample);

        VoronoiData InitialData { get; }
    }
}