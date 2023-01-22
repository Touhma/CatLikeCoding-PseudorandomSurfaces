using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

namespace _Utils.Interfaces
{
    public interface INoise {
        Sample4  GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency);
    }
}