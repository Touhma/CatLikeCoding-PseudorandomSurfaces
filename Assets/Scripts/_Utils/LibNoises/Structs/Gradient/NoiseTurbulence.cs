﻿using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;

using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct NoiseTurbulence<G> : IGradient where G : struct, IGradient {

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x) => default(G).Evaluate(hash, x);

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y) => default(G).Evaluate(hash, x, y);

        public Sample4 Evaluate (SmallXXHash4 hash, float4 x, float4 y, float4 z) => default(G).Evaluate(hash, x, y, z);

        public Sample4 EvaluateCombined (Sample4 value) => default(G).EvaluateCombined(value);
    }
}