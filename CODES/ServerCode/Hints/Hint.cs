namespace Hints
{
	public abstract class Hint : global::Hints.DisplayableObject<global::Hints.SharedHintData>
	{
		private global::Hints.HintEffect[] _effects;

		protected global::Hints.HintParameter[] Parameters { get; private set; }

		protected Hint(global::Hints.HintParameter[] parameters, global::Hints.HintEffect[] effects, float durationScalar = 1f)
			: base(durationScalar)
		{
			_effects = effects;
			Parameters = parameters;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			_effects = global::Utils.Networking.HintEffectArrayReaderWriter.ReadHintEffectArray(reader);
			Parameters = global::Utils.Networking.HintParameterArrayReaderWriter.ReadHintParameterArray(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Utils.Networking.HintEffectArrayReaderWriter.WriteHintEffectArray(writer, _effects);
			global::Utils.Networking.HintParameterArrayReaderWriter.WriteHintParameterArray(writer, Parameters);
		}
	}
}
