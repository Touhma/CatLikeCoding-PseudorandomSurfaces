using Unity.Mathematics;

namespace _Utils.Interfaces
{
    public interface IVoronoiDistance {
        float4 GetDistance (float4 x);

        float4 GetDistance (float4 x, float4 y);

        float4 GetDistance (float4 x, float4 y, float4 z);

        float4x2 Finalize1D (float4x2 minima);

        float4x2 Finalize2D (float4x2 minima);

        float4x2 Finalize3D (float4x2 minima);
    }
}