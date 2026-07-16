public static class JsonSerialize
{
	static JsonSerialize()
	{
		global::Utf8Json.Resolvers.CompositeResolver.RegisterAndSetAsDefault(global::Utf8Json.Resolvers.GeneratedResolver.Instance, global::Utf8Json.Resolvers.BuiltinResolver.Instance, global::Utf8Json.Resolvers.EnumResolver.Default, global::Utf8Json.Unity.UnityResolver.Instance, global::Utf8Json.Resolvers.StandardResolver.Default);
	}

	public static string ToJson<T>(T value) where T : IJsonSerializable
	{
		return global::Utf8Json.JsonSerializer.ToJsonString(value);
	}

	public static T FromJson<T>(global::System.IO.Stream value) where T : IJsonSerializable
	{
		return global::Utf8Json.JsonSerializer.Deserialize<T>(value);
	}

	public static T FromJson<T>(byte[] value) where T : IJsonSerializable
	{
		return global::Utf8Json.JsonSerializer.Deserialize<T>(value);
	}

	public static T FromJson<T>(byte[] value, int offset) where T : IJsonSerializable
	{
		return global::Utf8Json.JsonSerializer.Deserialize<T>(value, offset);
	}

	public static T FromJson<T>(string value) where T : IJsonSerializable
	{
		return global::Utf8Json.JsonSerializer.Deserialize<T>(value);
	}

	public static T FromFile<T>(string path) where T : IJsonSerializable
	{
		using (global::System.IO.FileStream stream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read, global::System.IO.FileShare.Read))
		{
			return global::Utf8Json.JsonSerializer.Deserialize<T>(stream);
		}
	}
}
