namespace GameCore
{
	[global::System.Serializable]
	public struct Log : global::System.IEquatable<global::GameCore.Log>
	{
		public string text;

		public global::UnityEngine.Color color;

		public bool nospace;

		public Log(string t, global::UnityEngine.Color c, bool b)
		{
			text = t;
			color = c;
			nospace = b;
		}

		public bool Equals(global::GameCore.Log other)
		{
			if (text == other.text && color == other.color)
			{
				return nospace == other.nospace;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is global::GameCore.Log other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((((text != null) ? text.GetHashCode() : 0) * 397) ^ color.GetHashCode()) * 397) ^ nospace.GetHashCode();
		}

		public static bool operator ==(global::GameCore.Log left, global::GameCore.Log right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(global::GameCore.Log left, global::GameCore.Log right)
		{
			return !left.Equals(right);
		}
	}
}
