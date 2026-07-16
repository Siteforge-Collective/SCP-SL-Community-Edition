namespace Utf8Json.Formatters
{
	public sealed class TypeFormatter : global::Utf8Json.IJsonFormatter<global::System.Type>, global::Utf8Json.IJsonFormatter
	{
		public static readonly global::Utf8Json.Formatters.TypeFormatter Default = new global::Utf8Json.Formatters.TypeFormatter();

		private static readonly global::System.Text.RegularExpressions.Regex SubtractFullNameRegex = new global::System.Text.RegularExpressions.Regex(", Version=\\d+.\\d+.\\d+.\\d+, Culture=\\w+, PublicKeyToken=\\w+");

		private bool serializeAssemblyQualifiedName;

		private bool deserializeSubtractAssemblyQualifiedName;

		private bool throwOnError;

		public TypeFormatter()
			: this(serializeAssemblyQualifiedName: true, deserializeSubtractAssemblyQualifiedName: true, throwOnError: true)
		{
		}

		public TypeFormatter(bool serializeAssemblyQualifiedName, bool deserializeSubtractAssemblyQualifiedName, bool throwOnError)
		{
			this.serializeAssemblyQualifiedName = serializeAssemblyQualifiedName;
			this.deserializeSubtractAssemblyQualifiedName = deserializeSubtractAssemblyQualifiedName;
			this.throwOnError = throwOnError;
		}

		public void Serialize(ref global::Utf8Json.JsonWriter writer, global::System.Type value, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else if (serializeAssemblyQualifiedName)
			{
				writer.WriteString(value.AssemblyQualifiedName);
			}
			else
			{
				writer.WriteString(value.FullName);
			}
		}

		public global::System.Type Deserialize(ref global::Utf8Json.JsonReader reader, global::Utf8Json.IJsonFormatterResolver formatterResolver)
		{
			if (reader.ReadIsNull())
			{
				return null;
			}
			string text = reader.ReadString();
			if (deserializeSubtractAssemblyQualifiedName)
			{
				text = SubtractFullNameRegex.Replace(text, "");
			}
			return global::System.Type.GetType(text, throwOnError);
		}
	}
}
