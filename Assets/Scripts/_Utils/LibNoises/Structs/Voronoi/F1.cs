using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct F1 : IVoronoiFunction {

        public Sample4 Evaluate (VoronoiData data) => data.a;
    }
}