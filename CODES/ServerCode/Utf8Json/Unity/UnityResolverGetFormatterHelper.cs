namespace Utf8Json.Unity
{
	internal static class UnityResolverGetFormatterHelper
	{
		private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

		static UnityResolverGetFormatterHelper()
		{
			lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(7)
			{
				{
					typeof(global::UnityEngine.Vector2),
					0
				},
				{
					typeof(global::UnityEngine.Vector3),
					1
				},
				{
					typeof(global::UnityEngine.Vector4),
					2
				},
				{
					typeof(global::UnityEngine.Quaternion),
					3
				},
				{
					typeof(global::UnityEngine.Color),
					4
				},
				{
					typeof(global::UnityEngine.Bounds),
					5
				},
				{
					typeof(global::UnityEngine.Rect),
					6
				},
				{
					typeof(global::UnityEngine.Vector2[]),
					7
				},
				{
					typeof(global::UnityEngine.Vector3[]),
					8
				},
				{
					typeof(global::UnityEngine.Vector4[]),
					9
				},
				{
					typeof(global::UnityEngine.Quaternion[]),
					10
				},
				{
					typeof(global::UnityEngine.Color[]),
					11
				},
				{
					typeof(global::UnityEngine.Bounds[]),
					12
				},
				{
					typeof(global::UnityEngine.Rect[]),
					13
				},
				{
					typeof(global::UnityEngine.Vector2?),
					14
				},
				{
					typeof(global::UnityEngine.Vector3?),
					15
				},
				{
					typeof(global::UnityEngine.Vector4?),
					16
				},
				{
					typeof(global::UnityEngine.Quaternion?),
					17
				},
				{
					typeof(global::UnityEngine.Color?),
					18
				},
				{
					typeof(global::UnityEngine.Bounds?),
					19
				},
				{
					typeof(global::UnityEngine.Rect?),
					20
				}
			};
		}

		internal static object GetFormatter(global::System.Type t)
		{
			if (!lookup.TryGetValue(t, out var value))
			{
				return null;
			}
			switch (value)
			{
			case 0:
				return new global::Utf8Json.Unity.Vector2Formatter();
			case 1:
				return new global::Utf8Json.Unity.Vector3Formatter();
			case 2:
				return new global::Utf8Json.Unity.Vector4Formatter();
			case 3:
				return new global::Utf8Json.Unity.QuaternionFormatter();
			case 4:
				return new global::Utf8Json.Unity.ColorFormatter();
			case 5:
				return new global::Utf8Json.Unity.BoundsFormatter();
			case 6:
				return new global::Utf8Json.Unity.RectFormatter();
			case 7:
				return new global::Utf8Json.Formatters.ArrayFormatter<global::UnityEngine.Vector2>();
			case 8:
				return new global::Utf8Json.Formatters.ArrayFormatter<global::UnityEngine.Vector3>();
			case 9:
				return new global::Utf8Json.Formatters.ArrayFormatter<global::UnityEngine.Vector4>();
			case 10:
				return new global::Utf8Json.Formatters.ArrayFormatter<global::UnityEngine.Quaternion>();
			case 11:
				return new global::Utf8Json.Formatters.ArrayFormatter<global::UnityEngine.Color>();
			case 12:
				return new global::Utf8Json.Formatters.ArrayFormatter<global::UnityEngine.Bounds>();
			case 13:
				return new global::Utf8Json.Formatters.ArrayFormatter<global::UnityEngine.Rect>();
			case 14:
				return new global::Utf8Json.Formatters.StaticNullableFormatter<global::UnityEngine.Vector2>(new global::Utf8Json.Unity.Vector2Formatter());
			case 15:
				return new global::Utf8Json.Formatters.StaticNullableFormatter<global::UnityEngine.Vector3>(new global::Utf8Json.Unity.Vector3Formatter());
			case 16:
				return new global::Utf8Json.Formatters.StaticNullableFormatter<global::UnityEngine.Vector4>(new global::Utf8Json.Unity.Vector4Formatter());
			case 17:
				return new global::Utf8Json.Formatters.StaticNullableFormatter<global::UnityEngine.Quaternion>(new global::Utf8Json.Unity.QuaternionFormatter());
			case 18:
				return new global::Utf8Json.Formatters.StaticNullableFormatter<global::UnityEngine.Color>(new global::Utf8Json.Unity.ColorFormatter());
			case 19:
				return new global::Utf8Json.Formatters.StaticNullableFormatter<global::UnityEngine.Bounds>(new global::Utf8Json.Unity.BoundsFormatter());
			case 20:
				return new global::Utf8Json.Formatters.StaticNullableFormatter<global::UnityEngine.Rect>(new global::Utf8Json.Unity.RectFormatter());
			default:
				return null;
			}
		}
	}
}
