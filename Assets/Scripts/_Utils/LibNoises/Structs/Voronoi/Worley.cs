using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Worley : IVoronoiDistance {

        public Sample4 GetDistance (float4 x) => new Sample4 {
            v = abs(x),
            dx = select(-1f, 1f, x < 0f)
        };

        public Sample4 GetDistance (float4 x, float4 y) => GetDistance(x, 0f, y);

        public Sample4 GetDistance (float4 x, float4 y, float4 z) => new Sample4 {
            v = x * x + y * y + z * z,
            dx = x,
            dy = y,
            dz = z
        };

        public VoronoiData Finalize1D (VoronoiData data) => data;

        public VoronoiData Finalize2D (VoronoiData data) => Finalize3D(data);

        public VoronoiData Finalize3D (VoronoiData data) {
            bool4 keepA = data.a.v < 1f;
            data.a.v = select(1f, sqrt(data.a.v), keepA);
            data.a.dx = select(0f, -data.a.dx / data.a.v, keepA);
            data.a.dy = select(0f, -data.a.dy / data.a.v, keepA);
            data.a.dz = select(0f, -data.a.dz / data.a.v, keepA);

            bool4 keepB = data.b.v < 1f;
            data.b.v = select(1f, sqrt(data.b.v), keepB);
            data.b.dx = select(0f, -data.b.dx / data.b.v, keepB);
            data.b.dy = select(0f, -data.b.dy / data.b.v, keepB);
            data.b.dz = select(0f, -data.b.dz / data.b.v, keepB);
            return data;
        }

        public VoronoiData UpdateVoronoiData (VoronoiData data, Sample4 sample) {
            bool4 newMinimum = sample.v < data.a.v;
            data.b = VoronoiHelper.Select(
                VoronoiHelper.Select(data.b, sample, sample.v < data.b.v),
                data.a,
                newMinimum
            );
            data.a = VoronoiHelper.Select(data.a, sample, newMinimum);
            return data;
        }

        public VoronoiData InitialData => new VoronoiData {
            a = new Sample4 { v = 2f },
            b = new Sample4 { v = 2f }
        };
    }
}