using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;
namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Chebyshev : IVoronoiDistance {

        public Sample4  GetDistance (float4 x) => default(Worley).GetDistance(x);

        public Sample4 GetDistance (float4 x, float4 y) { 
            bool4 keepX = abs(x) > abs(y);
            return new Sample4 {
                v = select(abs(y), abs(x), keepX),
                dx = select(0f, select(-1f, 1f, x < 0f), keepX),
                dz = select(select(-1f, 1f, y < 0f), 0f, keepX)
            };
        }

        public Sample4  GetDistance (float4 x, float4 y, float4 z){
            bool4 keepX = abs(x) > abs(y) & abs(x) > abs(z);
            bool4 keepY = abs(y) > abs(z);
            return new Sample4 {
                v = select(select(abs(z), abs(y), keepY), abs(x), keepX),
                dx = select(0f, select(-1f, 1f, x < 0f), keepX),
                dy = select(select(0f, select(-1f, 1f, y < 0f), keepY), 0f, keepX),
                dz = select(select(select(-1f, 1f, z < 0f), 0f, keepY), 0f, keepX)
            };
        }

        public VoronoiData Finalize1D (VoronoiData data) => data;

        public VoronoiData Finalize2D (VoronoiData data) => data;

        public VoronoiData Finalize3D (VoronoiData data) => data;
        
        public VoronoiData UpdateVoronoiData (VoronoiData data, Sample4 sample) => default(Worley).UpdateVoronoiData(data, sample);

        public VoronoiData InitialData => default(Worley).InitialData;
    }
}