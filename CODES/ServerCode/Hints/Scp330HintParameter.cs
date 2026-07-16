namespace Hints
{
	public class Scp330HintParameter : global::Hints.IdHintParameter
	{
		public static global::Hints.Scp330HintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.Scp330HintParameter scp330HintParameter = new global::Hints.Scp330HintParameter();
			scp330HintParameter.Deserialize(reader);
			return scp330HintParameter;
		}

		private Scp330HintParameter()
		{
		}

		public Scp330HintParameter(global::InventorySystem.Items.Usables.Scp330.Scp330Translations.Entry index)
			: base((byte)index)
		{
		}
	}
}
