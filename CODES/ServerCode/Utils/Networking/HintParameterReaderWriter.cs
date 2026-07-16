namespace Utils.Networking
{
	public static class HintParameterReaderWriter
	{
		public enum HintParameterType : byte
		{
			Text = 0,
			Timespan = 1,
			Ammo = 2,
			Item = 3,
			ItemCategory = 4,
			Byte = 5,
			SByte = 6,
			Short = 7,
			UShort = 8,
			Int = 9,
			UInt = 10,
			Long = 11,
			ULong = 12,
			Float = 13,
			Double = 14,
			PackedLong = 15,
			PackedULong = 16,
			Scp330Hint = 17
		}

		public static global::Hints.HintParameter ReadHintParameter(this global::Mirror.NetworkReader reader)
		{
			byte b = reader.ReadByte();
			global::System.Func<global::Mirror.NetworkReader, global::Hints.HintParameter> func;
			switch ((global::Utils.Networking.HintParameterReaderWriter.HintParameterType)b)
			{
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Text:
				func = global::Hints.StringHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Timespan:
				func = global::Hints.TimespanHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Ammo:
				func = global::Hints.AmmoHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Item:
				func = global::Hints.ItemHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.ItemCategory:
				func = global::Hints.ItemCategoryHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Byte:
				func = global::Hints.ByteHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.SByte:
				func = global::Hints.SByteHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Short:
				func = global::Hints.ShortHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.UShort:
				func = global::Hints.UShortHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Int:
				func = global::Hints.IntHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.UInt:
				func = global::Hints.UIntHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Long:
				func = global::Hints.LongHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.ULong:
				func = global::Hints.ULongHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Float:
				func = global::Hints.FloatHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Double:
				func = global::Hints.DoubleHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.PackedLong:
				func = global::Hints.PackedLongHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.PackedULong:
				func = global::Hints.PackedULongHintParameter.FromNetwork;
				break;
			case global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Scp330Hint:
				func = global::Hints.Scp330HintParameter.FromNetwork;
				break;
			default:
				global::UnityEngine.Debug.LogWarning($"Received malformed hint parameter (type {b}).");
				return null;
			}
			return func(reader);
		}

		public static void WriteHintParameter(this global::Mirror.NetworkWriter writer, global::Hints.HintParameter parameter)
		{
			if (parameter == null)
			{
				throw new global::System.ArgumentNullException("parameter");
			}
			if (parameter != null)
			{
				global::Utils.Networking.HintParameterReaderWriter.HintParameterType value;
				if (!(parameter is global::Hints.StringHintParameter))
				{
					if (!(parameter is global::Hints.TimespanHintParameter))
					{
						if (!(parameter is global::Hints.AmmoHintParameter))
						{
							if (!(parameter is global::Hints.ItemHintParameter))
							{
								if (!(parameter is global::Hints.ItemCategoryHintParameter))
								{
									if (!(parameter is global::Hints.ByteHintParameter))
									{
										if (!(parameter is global::Hints.SByteHintParameter))
										{
											if (!(parameter is global::Hints.ShortHintParameter))
											{
												if (!(parameter is global::Hints.UShortHintParameter))
												{
													if (!(parameter is global::Hints.IntHintParameter))
													{
														if (!(parameter is global::Hints.UIntHintParameter))
														{
															if (!(parameter is global::Hints.LongHintParameter))
															{
																if (!(parameter is global::Hints.ULongHintParameter))
																{
																	if (!(parameter is global::Hints.FloatHintParameter))
																	{
																		if (!(parameter is global::Hints.DoubleHintParameter))
																		{
																			if (!(parameter is global::Hints.PackedLongHintParameter))
																			{
																				if (!(parameter is global::Hints.PackedULongHintParameter))
																				{
																					if (!(parameter is global::Hints.Scp330HintParameter))
																					{
																						goto IL_0102;
																					}
																					value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Scp330Hint;
																				}
																				else
																				{
																					value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.PackedULong;
																				}
																			}
																			else
																			{
																				value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.PackedLong;
																			}
																		}
																		else
																		{
																			value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Double;
																		}
																	}
																	else
																	{
																		value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Float;
																	}
																}
																else
																{
																	value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.ULong;
																}
															}
															else
															{
																value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Long;
															}
														}
														else
														{
															value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.UInt;
														}
													}
													else
													{
														value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Int;
													}
												}
												else
												{
													value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.UShort;
												}
											}
											else
											{
												value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Short;
											}
										}
										else
										{
											value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.SByte;
										}
									}
									else
									{
										value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Byte;
									}
								}
								else
								{
									value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.ItemCategory;
								}
							}
							else
							{
								value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Item;
							}
						}
						else
						{
							value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Ammo;
						}
					}
					else
					{
						value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Timespan;
					}
				}
				else
				{
					value = global::Utils.Networking.HintParameterReaderWriter.HintParameterType.Text;
				}
				writer.WriteByte((byte)value);
				parameter.Serialize(writer);
				return;
			}
			goto IL_0102;
			IL_0102:
			throw new global::System.ArgumentException("Hint parameter was of an unknown type. This type should be added to the pattern switch (needed for polymorphism to work).", "parameter");
		}
	}
}
