using _Utils.NoisesLib.NoisesStructs;
using _Utils.NoisesLib.NoisesStructs.Commons;

namespace _Utils.Interfaces
{
    public interface IVoronoiFunction {
        Sample4  Evaluate (VoronoiData  data);
    }
}