using Unity.Mathematics;

namespace _Utils.Interfaces
{
    public interface IGradient
    {
        float4 Evaluate (SmallXXHash4 hash, float4 x);

        float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y);

        float4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z);
        
        float4 EvaluateCombined (float4 value);
    }
}