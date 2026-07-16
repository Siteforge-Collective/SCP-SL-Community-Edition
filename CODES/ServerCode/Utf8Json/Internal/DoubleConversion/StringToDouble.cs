namespace Utf8Json.Internal.DoubleConversion
{
	internal static class StringToDouble
	{
		[global::System.ThreadStatic]
		private static byte[] copyBuffer;

		private const int kMaxExactDoubleIntegerDecimalDigits = 15;

		private const int kMaxUint64DecimalDigits = 19;

		private const int kMaxDecimalPower = 309;

		private const int kMinDecimalPower = -324;

		private const ulong kMaxUint64 = ulong.MaxValue;

		private static readonly double[] exact_powers_of_ten = new double[23]
		{
			1.0, 10.0, 100.0, 1000.0, 10000.0, 100000.0, 1000000.0, 10000000.0, 100000000.0, 1000000000.0,
			10000000000.0, 100000000000.0, 1000000000000.0, 10000000000000.0, 100000000000000.0, 1000000000000000.0, 10000000000000000.0, 1E+17, 1E+18, 1E+19,
			1E+20, 1E+21, 1E+22
		};

		private static readonly int kExactPowersOfTenSize = exact_powers_of_ten.Length;

		private const int kMaxSignificantDecimalDigits = 780;

		private static byte[] GetCopyBuffer()
		{
			if (copyBuffer == null)
			{
				copyBuffer = new byte[780];
			}
			return copyBuffer;
		}

		private static global::Utf8Json.Internal.DoubleConversion.Vector TrimLeadingZeros(global::Utf8Json.Internal.DoubleConversion.Vector buffer)
		{
			for (int i = 0; i < buffer.length(); i++)
			{
				if (buffer[i] != 48)
				{
					return buffer.SubVector(i, buffer.length());
				}
			}
			return new global::Utf8Json.Internal.DoubleConversion.Vector(buffer.bytes, buffer.start, 0);
		}

		private static global::Utf8Json.Internal.DoubleConversion.Vector TrimTrailingZeros(global::Utf8Json.Internal.DoubleConversion.Vector buffer)
		{
			for (int num = buffer.length() - 1; num >= 0; num--)
			{
				if (buffer[num] != 48)
				{
					return buffer.SubVector(0, num + 1);
				}
			}
			return new global::Utf8Json.Internal.DoubleConversion.Vector(buffer.bytes, buffer.start, 0);
		}

		private static void CutToMaxSignificantDigits(global::Utf8Json.Internal.DoubleConversion.Vector buffer, int exponent, byte[] significant_buffer, out int significant_exponent)
		{
			for (int i = 0; i < 779; i++)
			{
				significant_buffer[i] = buffer[i];
			}
			significant_buffer[779] = 49;
			significant_exponent = exponent + (buffer.length() - 780);
		}

		private static void TrimAndCut(global::Utf8Json.Internal.DoubleConversion.Vector buffer, int exponent, byte[] buffer_copy_space, int space_size, out global::Utf8Json.Internal.DoubleConversion.Vector trimmed, out int updated_exponent)
		{
			global::Utf8Json.Internal.DoubleConversion.Vector buffer2 = TrimLeadingZeros(buffer);
			global::Utf8Json.Internal.DoubleConversion.Vector vector = TrimTrailingZeros(buffer2);
			exponent += buffer2.length() - vector.length();
			if (vector.length() > 780)
			{
				CutToMaxSignificantDigits(vector, exponent, buffer_copy_space, out updated_exponent);
				trimmed = new global::Utf8Json.Internal.DoubleConversion.Vector(buffer_copy_space, 0, 780);
			}
			else
			{
				trimmed = vector;
				updated_exponent = exponent;
			}
		}

		private static ulong ReadUint64(global::Utf8Json.Internal.DoubleConversion.Vector buffer, out int number_of_read_digits)
		{
			ulong num = 0uL;
			int num2 = 0;
			while (num2 < buffer.length() && num <= 1844674407370955160L)
			{
				int num3 = buffer[num2++] - 48;
				num = 10 * num + (ulong)num3;
			}
			number_of_read_digits = num2;
			return num;
		}

		private static void ReadDiyFp(global::Utf8Json.Internal.DoubleConversion.Vector buffer, out global::Utf8Json.Internal.DoubleConversion.DiyFp result, out int remaining_decimals)
		{
			int number_of_read_digits;
			ulong num = ReadUint64(buffer, out number_of_read_digits);
			if (buffer.length() == number_of_read_digits)
			{
				result = new global::Utf8Json.Internal.DoubleConversion.DiyFp(num, 0);
				remaining_decimals = 0;
				return;
			}
			if (buffer[number_of_read_digits] >= 53)
			{
				num++;
			}
			int exponent = 0;
			result = new global::Utf8Json.Internal.DoubleConversion.DiyFp(num, exponent);
			remaining_decimals = buffer.length() - number_of_read_digits;
		}

		private static bool DoubleStrtod(global::Utf8Json.Internal.DoubleConversion.Vector trimmed, int exponent, out double result)
		{
			if (trimmed.length() <= 15)
			{
				int number_of_read_digits;
				if (exponent < 0 && -exponent < kExactPowersOfTenSize)
				{
					result = ReadUint64(trimmed, out number_of_read_digits);
					result /= exact_powers_of_ten[-exponent];
					return true;
				}
				if (0 <= exponent && exponent < kExactPowersOfTenSize)
				{
					result = ReadUint64(trimmed, out number_of_read_digits);
					result *= exact_powers_of_ten[exponent];
					return true;
				}
				int num = 15 - trimmed.length();
				if (0 <= exponent && exponent - num < kExactPowersOfTenSize)
				{
					result = ReadUint64(trimmed, out number_of_read_digits);
					result *= exact_powers_of_ten[num];
					result *= exact_powers_of_ten[exponent - num];
					return true;
				}
			}
			result = 0.0;
			return false;
		}

		private static global::Utf8Json.Internal.DoubleConversion.DiyFp AdjustmentPowerOfTen(int exponent)
		{
			switch (exponent)
			{
			case 1:
				return new global::Utf8Json.Internal.DoubleConversion.DiyFp(11529215046068469760uL, -60);
			case 2:
				return new global::Utf8Json.Internal.DoubleConversion.DiyFp(14411518807585587200uL, -57);
			case 3:
				return new global::Utf8Json.Internal.DoubleConversion.DiyFp(18014398509481984000uL, -54);
			case 4:
				return new global::Utf8Json.Internal.DoubleConversion.DiyFp(11258999068426240000uL, -50);
			case 5:
				return new global::Utf8Json.Internal.DoubleConversion.DiyFp(14073748835532800000uL, -47);
			case 6:
				return new global::Utf8Json.Internal.DoubleConversion.DiyFp(17592186044416000000uL, -44);
			case 7:
				return new global::Utf8Json.Internal.DoubleConversion.DiyFp(10995116277760000000uL, -40);
			default:
				throw new global::System.Exception("unreached code.");
			}
		}

		private static bool DiyFpStrtod(global::Utf8Json.Internal.DoubleConversion.Vector buffer, int exponent, out double result)
		{
			ReadDiyFp(buffer, out var result2, out var remaining_decimals);
			exponent += remaining_decimals;
			ulong num = (ulong)(int)((remaining_decimals != 0) ? 4u : 0u);
			int e = result2.e;
			result2.Normalize();
			num <<= e - result2.e;
			if (exponent < -348)
			{
				result = 0.0;
				return true;
			}
			global::Utf8Json.Internal.DoubleConversion.PowersOfTenCache.GetCachedPowerForDecimalExponent(exponent, out var power, out var found_exponent);
			if (found_exponent != exponent)
			{
				int num2 = exponent - found_exponent;
				global::Utf8Json.Internal.DoubleConversion.DiyFp other = AdjustmentPowerOfTen(num2);
				result2.Multiply(ref other);
				if (19 - buffer.length() < num2)
				{
					num += 4;
				}
			}
			result2.Multiply(ref power);
			int num3 = 4;
			int num4 = ((num != 0L) ? 1 : 0);
			int num5 = 4;
			num += (ulong)(num3 + num4 + num5);
			e = result2.e;
			result2.Normalize();
			num <<= e - result2.e;
			int num6 = global::Utf8Json.Internal.DoubleConversion.Double.SignificandSizeForOrderOfMagnitude(64 + result2.e);
			int num7 = 64 - num6;
			if (num7 + 3 >= 64)
			{
				int num8 = num7 + 3 - 64 + 1;
				result2.f >>= num8;
				result2.e += num8;
				num = (num >> num8) + 1 + 8;
				num7 -= num8;
			}
			long num9 = 1L;
			ulong num10 = (ulong)((num9 << num7) - 1);
			ulong num11 = result2.f & num10;
			ulong num12 = (ulong)(num9 << num7 - 1);
			num11 *= 8;
			num12 *= 8;
			global::Utf8Json.Internal.DoubleConversion.DiyFp d = new global::Utf8Json.Internal.DoubleConversion.DiyFp(result2.f >> num7, result2.e + num7);
			if (num11 >= num12 + num)
			{
				d.f++;
			}
			result = new global::Utf8Json.Internal.DoubleConversion.Double(d).value();
			if (num12 - num < num11 && num11 < num12 + num)
			{
				return false;
			}
			return true;
		}

		private static bool ComputeGuess(global::Utf8Json.Internal.DoubleConversion.Vector trimmed, int exponent, out double guess)
		{
			if (trimmed.length() == 0)
			{
				guess = 0.0;
				return true;
			}
			if (exponent + trimmed.length() - 1 >= 309)
			{
				guess = global::Utf8Json.Internal.DoubleConversion.Double.Infinity();
				return true;
			}
			if (exponent + trimmed.length() <= -324)
			{
				guess = 0.0;
				return true;
			}
			if (DoubleStrtod(trimmed, exponent, out guess) || DiyFpStrtod(trimmed, exponent, out guess))
			{
				return true;
			}
			if (guess == global::Utf8Json.Internal.DoubleConversion.Double.Infinity())
			{
				return true;
			}
			return false;
		}

		public static double? Strtod(global::Utf8Json.Internal.DoubleConversion.Vector buffer, int exponent)
		{
			byte[] buffer_copy_space = GetCopyBuffer();
			TrimAndCut(buffer, exponent, buffer_copy_space, 780, out var trimmed, out var updated_exponent);
			exponent = updated_exponent;
			if (ComputeGuess(trimmed, exponent, out var guess))
			{
				return guess;
			}
			return null;
		}

		public static float? Strtof(global::Utf8Json.Internal.DoubleConversion.Vector buffer, int exponent)
		{
			byte[] buffer_copy_space = GetCopyBuffer();
			TrimAndCut(buffer, exponent, buffer_copy_space, 780, out var trimmed, out var updated_exponent);
			exponent = updated_exponent;
			double guess;
			bool flag = ComputeGuess(trimmed, exponent, out guess);
			float num = (float)guess;
			if ((double)num == guess)
			{
				return num;
			}
			double num2 = new global::Utf8Json.Internal.DoubleConversion.Double(guess).NextDouble();
			float num3 = (float)new global::Utf8Json.Internal.DoubleConversion.Double(guess).PreviousDouble();
			float num4 = (float)num2;
			float num5 = ((!flag) ? ((float)new global::Utf8Json.Internal.DoubleConversion.Double(num2).NextDouble()) : num4);
			if (num3 == num5)
			{
				return num;
			}
			return null;
		}
	}
}
