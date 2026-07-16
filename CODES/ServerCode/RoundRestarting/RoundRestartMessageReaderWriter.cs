namespace RoundRestarting
{
	public static class RoundRestartMessageReaderWriter
	{
		public static global::RoundRestarting.RoundRestartMessage ReadRoundRestartMessage(this global::Mirror.NetworkReader reader)
		{
			global::RoundRestarting.RoundRestartType roundRestartType = (global::RoundRestarting.RoundRestartType)reader.ReadByte();
			bool flag = false;
			bool extendedReconnectionPeriod = false;
			ushort newport = 0;
			switch (roundRestartType)
			{
			case global::RoundRestarting.RoundRestartType.FastRestart:
				return new global::RoundRestarting.RoundRestartMessage(roundRestartType, 0f, newport, reconnect: false, extendedReconnectionPeriod: false);
			case global::RoundRestarting.RoundRestartType.FullRestart:
				flag = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				if (flag)
				{
					extendedReconnectionPeriod = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				}
				break;
			case global::RoundRestarting.RoundRestartType.RedirectRestart:
				newport = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
				extendedReconnectionPeriod = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				break;
			}
			return new global::RoundRestarting.RoundRestartMessage(roundRestartType, global::Mirror.NetworkReaderExtensions.ReadSingle(reader), newport, flag, extendedReconnectionPeriod);
		}

		public static void WriteRoundRestartMessage(this global::Mirror.NetworkWriter writer, global::RoundRestarting.RoundRestartMessage msg)
		{
			writer.WriteByte((byte)msg.Type);
			switch (msg.Type)
			{
			case global::RoundRestarting.RoundRestartType.FastRestart:
				return;
			case global::RoundRestarting.RoundRestartType.FullRestart:
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, msg.Reconnect);
				if (msg.Reconnect)
				{
					global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, msg.ExtendedReconnectionPeriod);
				}
				break;
			case global::RoundRestarting.RoundRestartType.RedirectRestart:
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, msg.NewPort);
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, msg.ExtendedReconnectionPeriod);
				break;
			}
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, msg.TimeOffset);
		}
	}
}
