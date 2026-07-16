public static class AlphaWarheadSyncInfoSerializer
{
	public static void WritePickupSyncInfo(this global::Mirror.NetworkWriter writer, AlphaWarheadSyncInfo value)
	{
		global::Mirror.NetworkWriterExtensions.WriteDouble(writer, value.StartTime);
		if (value.StartTime != 0.0)
		{
			int num = value.ScenarioId;
			if (value.ResumeScenario)
			{
				num += 127;
			}
			writer.WriteByte((byte)num);
		}
	}

	public static AlphaWarheadSyncInfo ReadPickupSyncInfo(this global::Mirror.NetworkReader reader)
	{
		double num = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		if (num == 0.0)
		{
			return new AlphaWarheadSyncInfo
			{
				StartTime = 0.0
			};
		}
		int num2 = reader.ReadByte();
		bool resumeScenario;
		if (resumeScenario = num2 >= 127)
		{
			num2 -= 127;
		}
		return new AlphaWarheadSyncInfo
		{
			ResumeScenario = resumeScenario,
			ScenarioId = num2,
			StartTime = num
		};
	}
}
