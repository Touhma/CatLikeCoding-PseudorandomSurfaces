﻿using System.Runtime.CompilerServices;
using _Utils.Interfaces;
using _Utils.NoisesLib.NoisesStructs.Commons;
using _Utils.Structs.NoisesStructs;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace _Utils.NoisesLib.NoisesStructs
{
    public struct Lattice3D<L, G> : INoise
		where L : struct, ILattice where G : struct, IGradient {

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Sample4 GetNoise4 (float4x3 positions, SmallXXHash4 hash, int frequency) {
			L l = default;
			LatticeSpan4
				x = l.GetLatticeSpan4(positions.c0, frequency),
				y = l.GetLatticeSpan4(positions.c1, frequency),
				z = l.GetLatticeSpan4(positions.c2, frequency);

			SmallXXHash4
				h0 = hash.Eat(x.p0), h1 = hash.Eat(x.p1),
				h00 = h0.Eat(y.p0), h01 = h0.Eat(y.p1),
				h10 = h1.Eat(y.p0), h11 = h1.Eat(y.p1);

			G gradient = default;
			Sample4
				a = gradient.Evaluate(h00.Eat(z.p0), x.g0, y.g0, z.g0),
				b = gradient.Evaluate(h00.Eat(z.p1), x.g0, y.g0, z.g1),
				c = gradient.Evaluate(h01.Eat(z.p0), x.g0, y.g1, z.g0),
				d = gradient.Evaluate(h01.Eat(z.p1), x.g0, y.g1, z.g1),
				e = gradient.Evaluate(h10.Eat(z.p0), x.g1, y.g0, z.g0),
				f = gradient.Evaluate(h10.Eat(z.p1), x.g1, y.g0, z.g1),
				g = gradient.Evaluate(h11.Eat(z.p0), x.g1, y.g1, z.g0),
				h = gradient.Evaluate(h11.Eat(z.p1), x.g1, y.g1, z.g1);

			return gradient.EvaluateCombined(new Sample4 {
				v = lerp(
					lerp(lerp(a.v, b.v, z.t), lerp(c.v, d.v, z.t), y.t),
					lerp(lerp(e.v, f.v, z.t), lerp(g.v, h.v, z.t), y.t),
					x.t
				),
				dx = frequency * (
					lerp(
						lerp(lerp(a.dx, b.dx, z.t), lerp(c.dx, d.dx, z.t), y.t),
						lerp(lerp(e.dx, f.dx, z.t), lerp(g.dx, h.dx, z.t), y.t),
						x.t
					) + (
						lerp(lerp(e.v, f.v, z.t), lerp(g.v, h.v, z.t), y.t) -
						lerp(lerp(a.v, b.v, z.t), lerp(c.v, d.v, z.t), y.t)
					) * x.dt
				),
				dy = frequency * lerp(
					lerp(lerp(a.dy, b.dy, z.t), lerp(c.dy, d.dy, z.t), y.t) +
					(lerp(c.v, d.v, z.t) - lerp(a.v, b.v, z.t)) * y.dt,
					lerp(lerp(e.dy, f.dy, z.t), lerp(g.dy, h.dy, z.t), y.t) +
					(lerp(g.v, h.v, z.t) - lerp(e.v, f.v, z.t)) * y.dt,
					x.t
				),
				dz = frequency * lerp(
					lerp(
						lerp(a.dz, b.dz, z.t) + (b.v - a.v) * z.dt,
						lerp(c.dz, d.dz, z.t) + (d.v - c.v) * z.dt,
						y.t
					),
					lerp(
						lerp(e.dz, f.dz, z.t) + (f.v - e.v) * z.dt,
						lerp(g.dz, h.dz, z.t) + (h.v - g.v) * z.dt,
						y.t
					),
					x.t
				)
			});
		}
	}
}