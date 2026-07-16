namespace Utils.Networking
{
	public static class ArrayReaderWriter<TItem>
	{
		public delegate TItem ReadItem(global::Mirror.NetworkReader reader);

		public delegate void WriteItem(global::Mirror.NetworkWriter writer, TItem item);

		public static TItem[] ReadArray(global::Mirror.NetworkReader reader, global::Utils.Networking.ArrayReaderWriter<TItem>.ReadItem itemReader)
		{
			int num = global::Mirror.NetworkReaderExtensions.ReadInt32(reader);
			if (num < 0)
			{
				return null;
			}
			TItem[] array = new TItem[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = itemReader(reader);
			}
			return array;
		}

		public static void WriteArray(global::Mirror.NetworkWriter writer, global::System.Collections.Generic.IReadOnlyCollection<TItem> array, global::Utils.Networking.ArrayReaderWriter<TItem>.WriteItem itemWriter)
		{
			if (array != null)
			{
				global::Mirror.NetworkWriterExtensions.WriteInt32(writer, array.Count);
				{
					foreach (TItem item in array)
					{
						itemWriter(writer, item);
					}
					return;
				}
			}
			global::Mirror.NetworkWriterExtensions.WriteInt32(writer, -1);
		}
	}
}
