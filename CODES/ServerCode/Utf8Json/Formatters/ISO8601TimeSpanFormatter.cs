namespace Utf8Json.Formatters
{
	public sealed class ISO8601TimeSpanFormatter : global::Utf8Json.IJsonFormatter<global::System.TimeSpan>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.TimeSpan> Default = new global::Utf8Json.Formatters.ISO8601TimeSpanFormatter();

		private static byte[] minValue = global::Utf8Json.Internal.StringEncoding.UTF8.GetBytes("\"" + global::System.TimeSpan.MinValue.ToString() + "\"");

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.TimeSpan value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == global::System.TimeSpan.MinValue)
			{
				writer.WriteRaw(minValue);
				return;
			}
			bool num = value < global::System.TimeSpan.Zero;
			if (num)
			{
				value = value.Negate();
			}
			int days = value.Days;
			int hours = value.Hours;
			int minutes = value.Minutes;
			int seconds = value.Seconds;
			long num2 = value.Ticks % 10000000;
			writer.EnsureCapacity(19 + ((num2 != 0L) ? 8 : 0) + 6);
			writer.WriteRawUnsafe(34);
			if (num)
			{
				writer.WriteRawUnsafe(45);
			}
			if (days != 0)
			{
				writer.WriteInt32(days);
				writer.WriteRawUnsafe(46);
			}
			if (hours < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(hours);
			writer.WriteRawUnsafe(58);
			if (minutes < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(minutes);
			writer.WriteRawUnsafe(58);
			if (seconds < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(seconds);
			if (num2 != 0L)
			{
				writer.WriteRawUnsafe(46);
				writer.WriteInt64(num2);
			}
			writer.WriteRawUnsafe(34);
		}

		public global::System.TimeSpan Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentUnsafe();
			byte[] array = arraySegment.Array;
			int i = arraySegment.Offset;
			_ = arraySegment.Count;
			int num = arraySegment.Offset + arraySegment.Count;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int j = i; j < arraySegment.Count; j++)
			{
				if (array[j] == 46)
				{
					if (flag3)
					{
						break;
					}
					flag2 = true;
				}
				else if (array[j] == 58)
				{
					if (flag2)
					{
						flag = true;
					}
					flag3 = true;
				}
			}
			bool flag4 = false;
			if (array[i] == 45)
			{
				flag4 = true;
				i++;
			}
			int days = 0;
			if (flag)
			{
				byte[] array2 = global::Utf8Json.Internal.BufferPool.Default.Rent();
				try
				{
					for (; array[i] != 46; i++)
					{
						array2[days++] = array[i];
					}
					days = new global::Utf8Json.JsonReader(array2).ReadInt32();
					i++;
				}
				finally
				{
					global::Utf8Json.Internal.BufferPool.Default.Return(array2);
				}
			}
			int hours = (array[i++] - 48) * 10 + (array[i++] - 48);
			if (array[i++] == 58)
			{
				int minutes = (array[i++] - 48) * 10 + (array[i++] - 48);
				if (array[i++] == 58)
				{
					int seconds = (array[i++] - 48) * 10 + (array[i++] - 48);
					int num2 = 0;
					if (i < num && array[i] == 46)
					{
						i++;
						if (i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]))
						{
							num2 += (array[i] - 48) * 1000000;
							i++;
							if (i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]))
							{
								num2 += (array[i] - 48) * 100000;
								i++;
								if (i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]))
								{
									num2 += (array[i] - 48) * 10000;
									i++;
									if (i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]))
									{
										num2 += (array[i] - 48) * 1000;
										i++;
										if (i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]))
										{
											num2 += (array[i] - 48) * 100;
											i++;
											if (i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]))
											{
												num2 += (array[i] - 48) * 10;
												i++;
												if (i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]))
												{
													num2 += array[i] - 48;
													for (i++; i < num && global::Utf8Json.Internal.NumberConverter.IsNumber(array[i]); i++)
													{
													}
												}
											}
										}
									}
								}
							}
						}
					}
					global::System.TimeSpan timeSpan = new global::System.TimeSpan(days, hours, minutes, seconds);
					global::System.TimeSpan ts = global::System.TimeSpan.FromTicks(num2);
					if (!flag4)
					{
						return timeSpan.Add(ts);
					}
					return timeSpan.Negate().Subtract(ts);
				}
			}
			throw new global::System.InvalidOperationException("invalid datetime format. value:" + global::Utf8Json.Internal.StringEncoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count));
		}
	}
}
