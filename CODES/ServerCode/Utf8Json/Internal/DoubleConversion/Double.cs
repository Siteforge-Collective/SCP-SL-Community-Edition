namespace Utf8Json.Internal.DoubleConversion
{
	internal struct Double
	{
		public const ulong kSignMask = 9223372036854775808uL;

		public const ulong kExponentMask = 9218868437227405312uL;

		public const ulong kSignificandMask = 4503599627370495uL;

		public const ulong kHiddenBit = 4503599627370496uL;

		public const int kPhysicalSignificandSize = 52;

		public const int kSignificandSize = 53;

		private const int kExponentBias = 1075;

		private const int kDenormalExponent = -1074;

		private const int kMaxExponent = 972;

		private const ulong kInfinity = 9218868437227405312uL;

		private const ulong kNaN = 9221120237041090560uL;

		private ulong d64_;

		public Double(double d)
		{
			global::Utf8Json.Internal.DoubleConversion.UnionDoubleULong unionDoubleULong = new global::Utf8Json.Internal.DoubleConversion.UnionDoubleULong
			{
				d = d
			};
			d64_ = unionDoubleULong.u64;
		}

		public Double(global::Utf8Json.Internal.DoubleConversion.DiyFp d)
		{
			d64_ = DiyFpToUint64(d);
		}

		public global::Utf8Json.Internal.DoubleConversion.DiyFp AsDiyFp()
		{
			return new global::Utf8Json.Internal.DoubleConversion.DiyFp(Significand(), Exponent());
		}

		public global::Utf8Json.Internal.DoubleConversion.DiyFp AsNormalizedDiyFp()
		{
			ulong num = Significand();
			int num2 = Exponent();
			while ((num & 0x10000000000000L) == 0L)
			{
				num <<= 1;
				num2--;
			}
			num <<= 11;
			num2 -= 11;
			return new global::Utf8Json.Internal.DoubleConversion.DiyFp(num, num2);
		}

		public ulong AsUint64()
		{
			return d64_;
		}

		public double NextDouble()
		{
			if (d64_ == 9218868437227405312L)
			{
				return new global::Utf8Json.Internal.DoubleConversion.Double(9.218868437227405E+18).value();
			}
			if (Sign() < 0 && Significand() == 0L)
			{
				return 0.0;
			}
			if (Sign() < 0)
			{
				return new global::Utf8Json.Internal.DoubleConversion.Double(d64_ - 1).value();
			}
			return new global::Utf8Json.Internal.DoubleConversion.Double(d64_ + 1).value();
		}

		public double PreviousDouble()
		{
			if (d64_ == 18442240474082181120uL)
			{
				return 0.0 - Infinity();
			}
			if (Sign() < 0)
			{
				return new global::Utf8Json.Internal.DoubleConversion.Double(d64_ + 1).value();
			}
			if (Significand() == 0L)
			{
				return -0.0;
			}
			return new global::Utf8Json.Internal.DoubleConversion.Double(d64_ - 1).value();
		}

		public int Exponent()
		{
			if (IsDenormal())
			{
				return -1074;
			}
			return (int)((AsUint64() & 0x7FF0000000000000L) >> 52) - 1075;
		}

		public ulong Significand()
		{
			ulong num = AsUint64() & 0xFFFFFFFFFFFFFL;
			if (!IsDenormal())
			{
				return num + 4503599627370496L;
			}
			return num;
		}

		public bool IsDenormal()
		{
			return (AsUint64() & 0x7FF0000000000000L) == 0;
		}

		public bool IsSpecial()
		{
			return (AsUint64() & 0x7FF0000000000000L) == 9218868437227405312L;
		}

		public bool IsNan()
		{
			ulong num = AsUint64();
			if ((num & 0x7FF0000000000000L) == 9218868437227405312L)
			{
				return (num & 0xFFFFFFFFFFFFFL) != 0;
			}
			return false;
		}

		public bool IsInfinite()
		{
			ulong num = AsUint64();
			if ((num & 0x7FF0000000000000L) == 9218868437227405312L)
			{
				return (num & 0xFFFFFFFFFFFFFL) == 0;
			}
			return false;
		}

		public int Sign()
		{
			if ((AsUint64() & 0x8000000000000000uL) != 0L)
			{
				return -1;
			}
			return 1;
		}

		public global::Utf8Json.Internal.DoubleConversion.DiyFp UpperBoundary()
		{
			return new global::Utf8Json.Internal.DoubleConversion.DiyFp(Significand() * 2 + 1, Exponent() - 1);
		}

		public void NormalizedBoundaries(out global::Utf8Json.Internal.DoubleConversion.DiyFp out_m_minus, out global::Utf8Json.Internal.DoubleConversion.DiyFp out_m_plus)
		{
			global::Utf8Json.Internal.DoubleConversion.DiyFp diyFp = AsDiyFp();
			global::Utf8Json.Internal.DoubleConversion.DiyFp a = new global::Utf8Json.Internal.DoubleConversion.DiyFp((diyFp.f << 1) + 1, diyFp.e - 1);
			global::Utf8Json.Internal.DoubleConversion.DiyFp diyFp2 = global::Utf8Json.Internal.DoubleConversion.DiyFp.Normalize(ref a);
			global::Utf8Json.Internal.DoubleConversion.DiyFp diyFp3 = ((!LowerBoundaryIsCloser()) ? new global::Utf8Json.Internal.DoubleConversion.DiyFp((diyFp.f << 1) - 1, diyFp.e - 1) : new global::Utf8Json.Internal.DoubleConversion.DiyFp((diyFp.f << 2) - 1, diyFp.e - 2));
			diyFp3.f <<= diyFp3.e - diyFp2.e;
			diyFp3.e = diyFp2.e;
			out_m_plus = diyFp2;
			out_m_minus = diyFp3;
		}

		public bool LowerBoundaryIsCloser()
		{
			if ((AsUint64() & 0xFFFFFFFFFFFFFL) == 0)
			{
				return Exponent() != -1074;
			}
			return false;
		}

		public double value()
		{
			global::Utf8Json.Internal.DoubleConversion.UnionDoubleULong unionDoubleULong = new global::Utf8Json.Internal.DoubleConversion.UnionDoubleULong
			{
				u64 = d64_
			};
			return unionDoubleULong.d;
		}

		public static int SignificandSizeForOrderOfMagnitude(int order)
		{
			if (order >= -1021)
			{
				return 53;
			}
			if (order <= -1074)
			{
				return 0;
			}
			return order - -1074;
		}

		public static double Infinity()
		{
			return new global::Utf8Json.Internal.DoubleConversion.Double(9.218868437227405E+18).value();
		}

		public static double NaN()
		{
			return new global::Utf8Json.Internal.DoubleConversion.Double(9.221120237041091E+18).value();
		}

		public static ulong DiyFpToUint64(global::Utf8Json.Internal.DoubleConversion.DiyFp diy_fp)
		{
			ulong num = diy_fp.f;
			int num2 = diy_fp.e;
			while (num > 9007199254740991L)
			{
				num >>= 1;
				num2++;
			}
			if (num2 >= 972)
			{
				return 9218868437227405312uL;
			}
			if (num2 < -1074)
			{
				return 0uL;
			}
			while (num2 > -1074 && (num & 0x10000000000000L) == 0L)
			{
				num <<= 1;
				num2--;
			}
			ulong num3 = (ulong)((num2 != -1074 || (num & 0x10000000000000L) != 0L) ? (num2 + 1075) : 0);
			return (num & 0xFFFFFFFFFFFFFL) | (num3 << 52);
		}
	}
}
