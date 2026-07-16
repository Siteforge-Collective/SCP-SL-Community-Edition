namespace Hints
{
	public class TranslationHint : global::Hints.FormattableHint<global::Hints.TranslationHint>
	{
		public const string TranslationFile = "GameHints";

		protected global::Hints.HintTranslations Translation { get; private set; }

		public static global::Hints.TranslationHint FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.TranslationHint translationHint = new global::Hints.TranslationHint();
			translationHint.Deserialize(reader);
			return translationHint;
		}

		private TranslationHint()
			: base((global::Hints.HintParameter[])null, (global::Hints.HintEffect[])null, 0f)
		{
		}

		public TranslationHint(global::Hints.HintTranslations translation, global::Hints.HintParameter[] parameters = null, global::Hints.HintEffect[] effects = null, float durationScalar = 3f)
			: base(parameters, effects, durationScalar)
		{
			Translation = translation;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			Translation = (global::Hints.HintTranslations)reader.ReadByte();
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			writer.WriteByte((byte)Translation);
		}
	}
}
