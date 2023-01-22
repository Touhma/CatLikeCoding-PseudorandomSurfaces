using Unity.Mathematics;

namespace _Utils.Interfaces
{
    public interface IVoronoiFunction {
        float4 Evaluate (float4x2 minima);
    }
}