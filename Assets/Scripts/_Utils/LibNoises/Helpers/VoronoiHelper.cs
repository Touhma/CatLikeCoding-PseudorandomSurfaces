using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib
{
    public static class VoronoiHelper
    {
        public static float4x2 UpdateVoronoiMinima (float4x2 minima, float4 distances) {
            bool4 newMinimum = distances < minima.c0;
            minima.c1 = select(
                select(minima.c1, distances, distances < minima.c1),
                minima.c0,
                newMinimum
            );
            minima.c0 = select(minima.c0, distances, newMinimum);
            return minima;
        }
    }
}