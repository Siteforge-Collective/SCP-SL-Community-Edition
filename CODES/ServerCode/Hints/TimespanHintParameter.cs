namespace Hints
{
	public class TimespanHintParameter : global::Hints.DoubleHintParameter
	{
		protected bool Negate { get; private set; }

		protected global::System.TimeSpan OffsetTime
		{
			get
			{
				global::System.TimeSpan result = global::System.TimeSpan.FromSeconds(base.Value - global::Mirror.NetworkTime.time);
				if (!Negate)
				{
					return result;
				}
				return result.Negate();
			}
		}

		public new static global::Hints.TimespanHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.TimespanHintParameter timespanHintParameter = new global::Hints.TimespanHintParameter();
			timespanHintParameter.Deserialize(reader);
			return timespanHintParameter;
		}

		public static global::Hints.TimespanHintParameter FromOffset(double offset, string format, bool negate)
		{
			return new global::Hints.TimespanHintParameter(global::Mirror.NetworkTime.time + offset, format, negate);
		}

		public static global::Hints.TimespanHintParameter FromOffset(global::System.TimeSpan offset, string format, bool negate)
		{
			return FromOffset(offset.TotalSeconds, format, negate);
		}

		protected TimespanHintParameter()
		{
		}

		public TimespanHintParameter(double sourceTime, string format, bool negate)
			: base(sourceTime, format)
		{
			Negate = negate;
		}

		public TimespanHintParameter(global::System.DateTimeOffset sourceTime, string format, bool negate)
			: this((sourceTime - global::System.DateTimeOffset.UtcNow).TotalSeconds, format, negate)
		{
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			Negate = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, Negate);
		}
	}
}
