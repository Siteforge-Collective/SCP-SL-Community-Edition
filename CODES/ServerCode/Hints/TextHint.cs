namespace Hints
{
	public class TextHint : global::Hints.FormattableHint<global::Hints.TextHint>
	{
		protected string Text { get; private set; }

		public static global::Hints.TextHint FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.TextHint textHint = new global::Hints.TextHint();
			textHint.Deserialize(reader);
			return textHint;
		}

		private TextHint()
			: base((global::Hints.HintParameter[])null, (global::Hints.HintEffect[])null, 0f)
		{
		}

		public TextHint(string text, global::Hints.HintParameter[] parameters = null, global::Hints.HintEffect[] effects = null, float durationScalar = 3f)
			: base(parameters, effects, durationScalar)
		{
			Text = text;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			Text = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, Text);
		}
	}
}
