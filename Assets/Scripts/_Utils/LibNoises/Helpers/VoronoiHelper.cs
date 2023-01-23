using _Utils.NoisesLib.NoisesStructs;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib
{
    public static class VoronoiHelper
    {
        public static VoronoiData UpdateVoronoiData(VoronoiData data, Sample4 sample)
        {
            bool4 newMinimum = sample.v < data.a.v;
            data.b = Select(
                Select(data.b, sample, sample.v < data.b.v),
                data.a,
                newMinimum
            );
            data.a = Select(data.a, sample, newMinimum);
            return data;
        }

        public static Sample4 Select(Sample4 f, Sample4 t, bool4 b) => new Sample4
        {
            v = select(f.v, t.v, b),
            dx = select(f.dx, t.dx, b),
            dy = select(f.dy, t.dy, b),
            dz = select(f.dz, t.dz, b)
        };
    }
}