using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct WorleySmooth : IVoronoiDistance {

		const float smoothLSE = 10f, smoothPoly = 0.25f;

		public Sample4 GetDistance (float4 x) => default(Worley).GetDistance(x);

		public Sample4 GetDistance (float4 x, float4 y) => GetDistance(x, 0f, y);

		public Sample4 GetDistance (float4 x, float4 y, float4 z) {
			float4 v = sqrt(x * x + y * y + z * z);
			return new Sample4 {
				v = v,
				dx = x / -v,
				dy = y / -v,
				dz = z / -v
			};
		}

		public VoronoiData Finalize1D (VoronoiData data) {
			data.a.dx /= data.a.v;
			data.a.v = log(data.a.v) / -smoothLSE;
			data.a = VoronoiHelper.Select(default, data.a.Smoothstep, data.a.v > 0f);
			data.b = VoronoiHelper.Select(default, data.b.Smoothstep, data.b.v > 0f);
			return data;
		}

		public VoronoiData Finalize2D (VoronoiData data) => Finalize3D(data);

		public VoronoiData Finalize3D (VoronoiData data) {
			data.a.dx /= data.a.v;
			data.a.dy /= data.a.v;
			data.a.dz /= data.a.v;
			data.a.v = log(data.a.v) / -smoothLSE;
			data.a = VoronoiHelper.Select(default, data.a.Smoothstep, data.a.v > 0f & data.a.v < 1f);
			data.b = VoronoiHelper.Select(default, data.b.Smoothstep, data.b.v > 0f & data.b.v < 1f);
			return data;
		}

		public VoronoiData UpdateVoronoiData (VoronoiData data, Sample4 sample) {
			float4 e = exp(-smoothLSE * sample.v);
			data.a.v += e;
			data.a.dx += e * sample.dx;
			data.a.dy += e * sample.dy;
			data.a.dz += e * sample.dz;

			float4 h = 1f - abs(data.b.v - sample.v) / smoothPoly;
			float4
				hdx = data.b.dx - sample.dx,
				hdy = data.b.dy - sample.dy,
				hdz = data.b.dz - sample.dz;
			bool4 ds = data.b.v - sample.v < 0f;
			hdx = select(-hdx, hdx, ds) * 0.5f * h;
			hdy = select(-hdy, hdy, ds) * 0.5f * h;
			hdz = select(-hdz, hdz, ds) * 0.5f * h;

			bool4 smooth = h > 0f;
			h = 0.25f * smoothPoly * h * h;

			data.b = VoronoiHelper.Select(data.b, sample, sample.v < data.b.v);
			data.b.v -= select(0f, h, smooth);
			data.b.dx -= select(0f, hdx, smooth);
			data.b.dy -= select(0f, hdy, smooth);
			data.b.dz -= select(0f, hdz, smooth);
			return data;
		}

		public VoronoiData InitialData => new VoronoiData {
			b = new Sample4 { v = 2f }
		};
	}
}