namespace Utf8Json.Formatters
{
	public sealed class ISO8601DateTimeFormatter : global::Utf8Json.IJsonFormatter<global::System.DateTime>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.IJsonFormatter<global::System.DateTime> Default = new global::Utf8Json.Formatters.ISO8601DateTimeFormatter();

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.DateTime value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			int year = value.Year;
			int month = value.Month;
			int day = value.Day;
			int hour = value.Hour;
			int minute = value.Minute;
			int second = value.Second;
			long num = value.Ticks % 10000000;
			switch (value.Kind)
			{
			case global::System.DateTimeKind.Local:
				writer.EnsureCapacity(21 + ((num != 0L) ? 8 : 0) + 6);
				break;
			case global::System.DateTimeKind.Utc:
				writer.EnsureCapacity(21 + ((num != 0L) ? 8 : 0) + 1);
				break;
			default:
				writer.EnsureCapacity(21 + ((num != 0L) ? 8 : 0));
				break;
			}
			writer.WriteRawUnsafe(34);
			if (year < 10)
			{
				writer.WriteRawUnsafe(48);
				writer.WriteRawUnsafe(48);
				writer.WriteRawUnsafe(48);
			}
			else if (year < 100)
			{
				writer.WriteRawUnsafe(48);
				writer.WriteRawUnsafe(48);
			}
			else if (year < 1000)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(year);
			writer.WriteRawUnsafe(45);
			if (month < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(month);
			writer.WriteRawUnsafe(45);
			if (day < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(day);
			writer.WriteRawUnsafe(84);
			if (hour < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(hour);
			writer.WriteRawUnsafe(58);
			if (minute < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(minute);
			writer.WriteRawUnsafe(58);
			if (second < 10)
			{
				writer.WriteRawUnsafe(48);
			}
			writer.WriteInt32(second);
			if (num != 0L)
			{
				writer.WriteRawUnsafe(46);
				if (num < 10)
				{
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
				}
				else if (num < 100)
				{
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
				}
				else if (num < 1000)
				{
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
				}
				else if (num < 10000)
				{
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
				}
				else if (num < 100000)
				{
					writer.WriteRawUnsafe(48);
					writer.WriteRawUnsafe(48);
				}
				else if (num < 1000000)
				{
					writer.WriteRawUnsafe(48);
				}
				writer.WriteInt64(num);
			}
			switch (value.Kind)
			{
			case global::System.DateTimeKind.Local:
			{
				global::System.TimeSpan timeSpan = global::System.TimeZoneInfo.Local.GetUtcOffset(value);
				bool flag = timeSpan < global::System.TimeSpan.Zero;
				if (flag)
				{
					timeSpan = timeSpan.Negate();
				}
				int hours = timeSpan.Hours;
				int minutes = timeSpan.Minutes;
				writer.WriteRawUnsafe((byte)(flag ? 45 : 43));
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
				break;
			}
			case global::System.DateTimeKind.Utc:
				writer.WriteRawUnsafe(90);
				break;
			}
			writer.WriteRawUnsafe(34);
		}

		public global::System.DateTime Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			global::System.ArraySegment<byte> arraySegment = reader.ReadStringSegmentUnsafe();
			byte[] array = arraySegment.Array;
			int i = arraySegment.Offset;
			int count = arraySegment.Count;
			int num = arraySegment.Offset + arraySegment.Count;
			if (count == 4)
			{
				return new global::System.DateTime((array[i++] - 48) * 1000 + (array[i++] - 48) * 100 + (array[i++] - 48) * 10 + (array[i++] - 48), 1, 1);
			}
			if (count == 7)
			{
				int year = (array[i++] - 48) * 1000 + (array[i++] - 48) * 100 + (array[i++] - 48) * 10 + (array[i++] - 48);
				if (array[i++] == 45)
				{
					int month = (array[i++] - 48) * 10 + (array[i++] - 48);
					return new global::System.DateTime(year, month, 1);
				}
			}
			else if (count == 10)
			{
				int year2 = (array[i++] - 48) * 1000 + (array[i++] - 48) * 100 + (array[i++] - 48) * 10 + (array[i++] - 48);
				if (array[i++] == 45)
				{
					int month2 = (array[i++] - 48) * 10 + (array[i++] - 48);
					if (array[i++] == 45)
					{
						int day = (array[i++] - 48) * 10 + (array[i++] - 48);
						return new global::System.DateTime(year2, month2, day);
					}
				}
			}
			else if (count >= 19)
			{
				int year3 = (array[i++] - 48) * 1000 + (array[i++] - 48) * 100 + (array[i++] - 48) * 10 + (array[i++] - 48);
				if (array[i++] == 45)
				{
					int month3 = (array[i++] - 48) * 10 + (array[i++] - 48);
					if (array[i++] == 45)
					{
						int day2 = (array[i++] - 48) * 10 + (array[i++] - 48);
						if (array[i++] == 84)
						{
							int hour = (array[i++] - 48) * 10 + (array[i++] - 48);
							if (array[i++] == 58)
							{
								int minute = (array[i++] - 48) * 10 + (array[i++] - 48);
								if (array[i++] == 58)
								{
									int second = (array[i++] - 48) * 10 + (array[i++] - 48);
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
									global::System.DateTimeKind kind = global::System.DateTimeKind.Unspecified;
									if (i < num && array[i] == 90)
									{
										kind = global::System.DateTimeKind.Utc;
									}
									else if ((i < num && array[i] == 45) || array[i] == 43)
									{
										if (i + 5 < num)
										{
											kind = global::System.DateTimeKind.Local;
											bool num3 = array[i++] == 45;
											int hours = (array[i++] - 48) * 10 + (array[i++] - 48);
											i++;
											int minutes = (array[i++] - 48) * 10 + (array[i++] - 48);
											global::System.TimeSpan value = new global::System.TimeSpan(hours, minutes, 0);
											if (num3)
											{
												value = value.Negate();
											}
											return new global::System.DateTime(year3, month3, day2, hour, minute, second, global::System.DateTimeKind.Utc).AddTicks(num2).Subtract(value).ToLocalTime();
										}
										goto IL_04a6;
									}
									return new global::System.DateTime(year3, month3, day2, hour, minute, second, kind).AddTicks(num2);
								}
							}
						}
					}
				}
			}
			goto IL_04a6;
			IL_04a6:
			throw new global::System.InvalidOperationException("invalid datetime format. value:" + global::Utf8Json.Internal.StringEncoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count));
		}
	}
}
