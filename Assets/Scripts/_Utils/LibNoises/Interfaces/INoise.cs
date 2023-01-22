using Unity.Mathematics;

namespace _Utils.Interfaces
{
    public interface INoise {
        float4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency);
    }
}