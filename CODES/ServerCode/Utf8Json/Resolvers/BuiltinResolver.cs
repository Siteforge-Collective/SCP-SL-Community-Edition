namespace Utf8Json.Resolvers
{
	public sealed class BuiltinResolver : global::Utf8Json.IJsonFormatterResolver
	{
		private static class FormatterCache<T>
		{
			public static readonly global::Utf8Json.IJsonFormatter<T> formatter;

			static FormatterCache()
			{
				formatter = (global::Utf8Json.IJsonFormatter<T>)global::Utf8Json.Resolvers.BuiltinResolver.BuiltinResolverGetFormatterHelper.GetFormatter(typeof(T));
			}
		}

		internal static class BuiltinResolverGetFormatterHelper
		{
			private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, object> formatterMap = new global::System.Collections.Generic.Dictionary<global::System.Type, object>
			{
				{
					typeof(short),
					global::Utf8Json.Formatters.Int16Formatter.Default
				},
				{
					typeof(int),
					global::Utf8Json.Formatters.Int32Formatter.Default
				},
				{
					typeof(long),
					global::Utf8Json.Formatters.Int64Formatter.Default
				},
				{
					typeof(ushort),
					global::Utf8Json.Formatters.UInt16Formatter.Default
				},
				{
					typeof(uint),
					global::Utf8Json.Formatters.UInt32Formatter.Default
				},
				{
					typeof(ulong),
					global::Utf8Json.Formatters.UInt64Formatter.Default
				},
				{
					typeof(float),
					global::Utf8Json.Formatters.SingleFormatter.Default
				},
				{
					typeof(double),
					global::Utf8Json.Formatters.DoubleFormatter.Default
				},
				{
					typeof(bool),
					global::Utf8Json.Formatters.BooleanFormatter.Default
				},
				{
					typeof(byte),
					global::Utf8Json.Formatters.ByteFormatter.Default
				},
				{
					typeof(sbyte),
					global::Utf8Json.Formatters.SByteFormatter.Default
				},
				{
					typeof(short?),
					global::Utf8Json.Formatters.NullableInt16Formatter.Default
				},
				{
					typeof(int?),
					global::Utf8Json.Formatters.NullableInt32Formatter.Default
				},
				{
					typeof(long?),
					global::Utf8Json.Formatters.NullableInt64Formatter.Default
				},
				{
					typeof(ushort?),
					global::Utf8Json.Formatters.NullableUInt16Formatter.Default
				},
				{
					typeof(uint?),
					global::Utf8Json.Formatters.NullableUInt32Formatter.Default
				},
				{
					typeof(ulong?),
					global::Utf8Json.Formatters.NullableUInt64Formatter.Default
				},
				{
					typeof(float?),
					global::Utf8Json.Formatters.NullableSingleFormatter.Default
				},
				{
					typeof(double?),
					global::Utf8Json.Formatters.NullableDoubleFormatter.Default
				},
				{
					typeof(bool?),
					global::Utf8Json.Formatters.NullableBooleanFormatter.Default
				},
				{
					typeof(byte?),
					global::Utf8Json.Formatters.NullableByteFormatter.Default
				},
				{
					typeof(sbyte?),
					global::Utf8Json.Formatters.NullableSByteFormatter.Default
				},
				{
					typeof(global::System.DateTime),
					global::Utf8Json.Formatters.ISO8601DateTimeFormatter.Default
				},
				{
					typeof(global::System.TimeSpan),
					global::Utf8Json.Formatters.ISO8601TimeSpanFormatter.Default
				},
				{
					typeof(global::System.DateTimeOffset),
					global::Utf8Json.Formatters.ISO8601DateTimeOffsetFormatter.Default
				},
				{
					typeof(global::System.DateTime?),
					new global::Utf8Json.Formatters.StaticNullableFormatter<global::System.DateTime>(global::Utf8Json.Formatters.ISO8601DateTimeFormatter.Default)
				},
				{
					typeof(global::System.TimeSpan?),
					new global::Utf8Json.Formatters.StaticNullableFormatter<global::System.TimeSpan>(global::Utf8Json.Formatters.ISO8601TimeSpanFormatter.Default)
				},
				{
					typeof(global::System.DateTimeOffset?),
					new global::Utf8Json.Formatters.StaticNullableFormatter<global::System.DateTimeOffset>(global::Utf8Json.Formatters.ISO8601DateTimeOffsetFormatter.Default)
				},
				{
					typeof(string),
					global::Utf8Json.Formatters.NullableStringFormatter.Default
				},
				{
					typeof(char),
					global::Utf8Json.Formatters.CharFormatter.Default
				},
				{
					typeof(char?),
					global::Utf8Json.Formatters.NullableCharFormatter.Default
				},
				{
					typeof(decimal),
					global::Utf8Json.Formatters.DecimalFormatter.Default
				},
				{
					typeof(decimal?),
					new global::Utf8Json.Formatters.StaticNullableFormatter<decimal>(global::Utf8Json.Formatters.DecimalFormatter.Default)
				},
				{
					typeof(global::System.Guid),
					global::Utf8Json.Formatters.GuidFormatter.Default
				},
				{
					typeof(global::System.Guid?),
					new global::Utf8Json.Formatters.StaticNullableFormatter<global::System.Guid>(global::Utf8Json.Formatters.GuidFormatter.Default)
				},
				{
					typeof(global::System.Uri),
					global::Utf8Json.Formatters.UriFormatter.Default
				},
				{
					typeof(global::System.Version),
					global::Utf8Json.Formatters.VersionFormatter.Default
				},
				{
					typeof(global::System.Text.StringBuilder),
					global::Utf8Json.Formatters.StringBuilderFormatter.Default
				},
				{
					typeof(global::System.Collections.BitArray),
					global::Utf8Json.Formatters.BitArrayFormatter.Default
				},
				{
					typeof(global::System.Type),
					global::Utf8Json.Formatters.TypeFormatter.Default
				},
				{
					typeof(byte[]),
					global::Utf8Json.Formatters.ByteArrayFormatter.Default
				},
				{
					typeof(short[]),
					global::Utf8Json.Formatters.Int16ArrayFormatter.Default
				},
				{
					typeof(int[]),
					global::Utf8Json.Formatters.Int32ArrayFormatter.Default
				},
				{
					typeof(long[]),
					global::Utf8Json.Formatters.Int64ArrayFormatter.Default
				},
				{
					typeof(ushort[]),
					global::Utf8Json.Formatters.UInt16ArrayFormatter.Default
				},
				{
					typeof(uint[]),
					global::Utf8Json.Formatters.UInt32ArrayFormatter.Default
				},
				{
					typeof(ulong[]),
					global::Utf8Json.Formatters.UInt64ArrayFormatter.Default
				},
				{
					typeof(float[]),
					global::Utf8Json.Formatters.SingleArrayFormatter.Default
				},
				{
					typeof(double[]),
					global::Utf8Json.Formatters.DoubleArrayFormatter.Default
				},
				{
					typeof(bool[]),
					global::Utf8Json.Formatters.BooleanArrayFormatter.Default
				},
				{
					typeof(sbyte[]),
					global::Utf8Json.Formatters.SByteArrayFormatter.Default
				},
				{
					typeof(char[]),
					global::Utf8Json.Formatters.CharArrayFormatter.Default
				},
				{
					typeof(string[]),
					global::Utf8Json.Formatters.NullableStringArrayFormatter.Default
				},
				{
					typeof(global::System.Collections.Generic.List<short>),
					new global::Utf8Json.Formatters.ListFormatter<short>()
				},
				{
					typeof(global::System.Collections.Generic.List<int>),
					new global::Utf8Json.Formatters.ListFormatter<int>()
				},
				{
					typeof(global::System.Collections.Generic.List<long>),
					new global::Utf8Json.Formatters.ListFormatter<long>()
				},
				{
					typeof(global::System.Collections.Generic.List<ushort>),
					new global::Utf8Json.Formatters.ListFormatter<ushort>()
				},
				{
					typeof(global::System.Collections.Generic.List<uint>),
					new global::Utf8Json.Formatters.ListFormatter<uint>()
				},
				{
					typeof(global::System.Collections.Generic.List<ulong>),
					new global::Utf8Json.Formatters.ListFormatter<ulong>()
				},
				{
					typeof(global::System.Collections.Generic.List<float>),
					new global::Utf8Json.Formatters.ListFormatter<float>()
				},
				{
					typeof(global::System.Collections.Generic.List<double>),
					new global::Utf8Json.Formatters.ListFormatter<double>()
				},
				{
					typeof(global::System.Collections.Generic.List<bool>),
					new global::Utf8Json.Formatters.ListFormatter<bool>()
				},
				{
					typeof(global::System.Collections.Generic.List<byte>),
					new global::Utf8Json.Formatters.ListFormatter<byte>()
				},
				{
					typeof(global::System.Collections.Generic.List<sbyte>),
					new global::Utf8Json.Formatters.ListFormatter<sbyte>()
				},
				{
					typeof(global::System.Collections.Generic.List<global::System.DateTime>),
					new global::Utf8Json.Formatters.ListFormatter<global::System.DateTime>()
				},
				{
					typeof(global::System.Collections.Generic.List<char>),
					new global::Utf8Json.Formatters.ListFormatter<char>()
				},
				{
					typeof(global::System.Collections.Generic.List<string>),
					new global::Utf8Json.Formatters.ListFormatter<string>()
				},
				{
					typeof(global::System.ArraySegment<byte>),
					global::Utf8Json.Formatters.ByteArraySegmentFormatter.Default
				},
				{
					typeof(global::System.ArraySegment<byte>?),
					new global::Utf8Json.Formatters.StaticNullableFormatter<global::System.ArraySegment<byte>>(global::Utf8Json.Formatters.ByteArraySegmentFormatter.Default)
				}
			};

			internal static object GetFormatter(global::System.Type t)
			{
				if (formatterMap.TryGetValue(t, out var value))
				{
					return value;
				}
				return null;
			}
		}

		public static readonly global::Utf8Json.IJsonFormatterResolver Instance = new global::Utf8Json.Resolvers.BuiltinResolver();

		private BuiltinResolver()
		{
		}

		public global::Utf8Json.IJsonFormatter<T> GetFormatter<T>()
		{
			return global::Utf8Json.Resolvers.BuiltinResolver.FormatterCache<T>.formatter;
		}
	}
}
