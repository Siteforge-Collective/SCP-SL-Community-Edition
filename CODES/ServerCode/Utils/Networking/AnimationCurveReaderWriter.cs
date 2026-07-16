namespace Utils.Networking
{
	public static class AnimationCurveReaderWriter
	{
		private struct NetworkKeyframe
		{
			public global::UnityEngine.Keyframe Keyframe { get; private set; }

			public bool Tangental { get; private set; }

			public bool Weighted { get; private set; }

			private static void GetFlagsFromOffset(ref byte bitOffset, out byte tangentalFlag, out byte weightedFlag)
			{
				tangentalFlag = (byte)(1 << (int)bitOffset++);
				weightedFlag = (byte)(1 << (int)bitOffset++);
			}

			public NetworkKeyframe(global::UnityEngine.Keyframe keyframe)
			{
				Keyframe = keyframe;
				Tangental = global::UnityEngine.Mathf.Abs(keyframe.inTangent) > float.Epsilon || global::UnityEngine.Mathf.Abs(keyframe.inTangent) > float.Epsilon;
				Weighted = global::UnityEngine.Mathf.Abs(keyframe.inWeight) > float.Epsilon || global::UnityEngine.Mathf.Abs(keyframe.outWeight) > float.Epsilon;
			}

			public void ReadMetaTable(in byte flag, ref byte bitOffset)
			{
				GetFlagsFromOffset(ref bitOffset, out var tangentalFlag, out var weightedFlag);
				Tangental = (flag & tangentalFlag) == tangentalFlag;
				Weighted = (flag & weightedFlag) == weightedFlag;
			}

			public void WriteMetaTable(ref byte flag, ref byte bitOffset)
			{
				GetFlagsFromOffset(ref bitOffset, out var tangentalFlag, out var weightedFlag);
				if (Tangental)
				{
					flag |= tangentalFlag;
				}
				if (Weighted)
				{
					flag |= weightedFlag;
				}
			}

			public void ReadData(global::Mirror.NetworkReader reader)
			{
				float time = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				float value = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				float inTangent;
				float outTangent;
				if (Tangental)
				{
					inTangent = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
					outTangent = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				}
				else
				{
					inTangent = 0f;
					outTangent = 0f;
				}
				float inWeight;
				float outWeight;
				if (Weighted)
				{
					inWeight = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
					outWeight = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
				}
				else
				{
					inWeight = 0f;
					outWeight = 0f;
				}
				Keyframe = new global::UnityEngine.Keyframe(time, value, inTangent, outTangent, inWeight, outWeight);
			}

			public void WriteData(global::Mirror.NetworkWriter writer)
			{
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Keyframe.time);
				global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Keyframe.value);
				if (Tangental)
				{
					global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Keyframe.inTangent);
					global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Keyframe.outTangent);
				}
				if (Weighted)
				{
					global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Keyframe.inWeight);
					global::Mirror.NetworkWriterExtensions.WriteSingle(writer, Keyframe.outWeight);
				}
			}
		}

		public const byte KeyCountOffset = 2;

		public static global::UnityEngine.AnimationCurve ReadAnimationCurve(this global::Mirror.NetworkReader reader)
		{
			global::Utils.Networking.AnimationCurveReaderWriter.NetworkKeyframe[] array = new global::Utils.Networking.AnimationCurveReaderWriter.NetworkKeyframe[reader.ReadByte() + 2];
			int num = 0;
			while (num < array.Length)
			{
				byte flag = reader.ReadByte();
				byte bitOffset = 0;
				do
				{
					array[num].ReadMetaTable(in flag, ref bitOffset);
					num++;
				}
				while (bitOffset < 8 && num < array.Length);
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ReadData(reader);
			}
			return new global::UnityEngine.AnimationCurve(global::System.Array.ConvertAll(array, (global::Utils.Networking.AnimationCurveReaderWriter.NetworkKeyframe input) => input.Keyframe))
			{
				postWrapMode = global::UnityEngine.WrapMode.Loop
			};
		}

		public static void WriteAnimationCurve(this global::Mirror.NetworkWriter writer, global::UnityEngine.AnimationCurve animationCurve)
		{
			if (animationCurve.length > 257)
			{
				throw new global::System.ArgumentException("Curve cannot have more than " + 257 + " keys.", "animationCurve");
			}
			global::Utils.Networking.AnimationCurveReaderWriter.NetworkKeyframe[] array = global::System.Array.ConvertAll(animationCurve.keys, (global::UnityEngine.Keyframe input) => new global::Utils.Networking.AnimationCurveReaderWriter.NetworkKeyframe(input));
			writer.WriteByte((byte)(array.Length - 2));
			int num = 0;
			while (num < array.Length)
			{
				byte flag = 0;
				byte bitOffset = 0;
				do
				{
					array[num].WriteMetaTable(ref flag, ref bitOffset);
					num++;
				}
				while (bitOffset < 8 && num < array.Length);
				writer.WriteByte(flag);
			}
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				array[num2].WriteData(writer);
			}
		}
	}
}
