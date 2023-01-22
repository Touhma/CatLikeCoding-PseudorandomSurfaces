using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

namespace _Utils.Interfaces
{
    public interface IGradient
    {
        Sample4 Evaluate (SmallXXHash4 hash, float4 x);

        Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y);

        Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z);
        
        Sample4 EvaluateCombined (Sample4 value);
    }
}