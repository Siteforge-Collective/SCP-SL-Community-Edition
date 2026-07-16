namespace Hints
{
	public class OutlineEffect : global::Hints.HintEffect
	{
		protected global::UnityEngine.Color32 OutlineColor { get; private set; }

		protected float OutlineWidth { get; private set; }

		public static global::Hints.OutlineEffect FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.OutlineEffect outlineEffect = new global::Hints.OutlineEffect();
			outlineEffect.Deserialize(reader);
			return outlineEffect;
		}

		private OutlineEffect()
		{
		}

		public OutlineEffect(global::UnityEngine.Color32 outlineColor, float outlineWidth, float startScalar = 0f, float durationScalar = 1f)
			: base(startScalar, durationScalar)
		{
			OutlineColor = outlineColor;
			OutlineWidth = outlineWidth;
		}

		public override void Deserialize(global::Mirror.NetworkReader reader)
		{
			base.Deserialize(reader);
			OutlineColor = global::Mirror.NetworkReaderExtensions.ReadColor32(reader);
			OutlineWidth = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
		}

		public override void Serialize(global::Mirror.NetworkWriter writer)
		{
			base.Serialize(writer);
			global::Mirror.NetworkWriterExtensions.WriteColor32(writer, OutlineColor);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, OutlineWidth);
		}
	}
}
