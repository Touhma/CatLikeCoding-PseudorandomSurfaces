using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs.Commons
{
    public struct Sample4
    {
        public float4 v, dx, dy, dz;
        
        public static implicit operator Sample4 (float4 v) => new Sample4 { v = v };
        
        public static Sample4 operator + (Sample4 a, Sample4 b) => new Sample4 {
            v = a.v + b.v,
            dx = a.dx + b.dx,
            dy = a.dy + b.dy,
            dz = a.dz + b.dz
        };
        
        public static Sample4 operator - (Sample4 a, Sample4 b) => new Sample4 {
            v = a.v - b.v,
            dx = a.dx - b.dx,
            dy = a.dy - b.dy,
            dz = a.dz - b.dz
        };
        
        public static Sample4 operator * (Sample4 a, float4 b) => new Sample4 {
            v = a.v * b,
            dx = a.dx * b,
            dy = a.dy * b,
            dz = a.dz * b
        };
		
        public static Sample4 operator * (float4 a, Sample4 b) => b * a;
        
        public static Sample4 operator / (Sample4 a, float4 b) => new Sample4 {
            v = a.v / b,
            dx = a.dx / b,
            dy = a.dy / b,
            dz = a.dz / b
        };
        
        public float4x3 Derivatives => float4x3(dx, dy, dz);
        
        public Sample4 Smoothstep {
            get {
                Sample4 s = this;
                float4 d = 6f * v * (1f - v);
                s.dx *= d;
                s.dy *= d;
                s.dz *= d;
                s.v *= v * (3f - 2f * v);
                return s;
            }
        }
    }
}