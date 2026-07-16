namespace Hints
{
	public class AmmoHintParameter : global::Hints.IdHintParameter
	{
		public static global::Hints.AmmoHintParameter FromNetwork(global::Mirror.NetworkReader reader)
		{
			global::Hints.AmmoHintParameter ammoHintParameter = new global::Hints.AmmoHintParameter();
			ammoHintParameter.Deserialize(reader);
			return ammoHintParameter;
		}

		private AmmoHintParameter()
		{
		}

		public AmmoHintParameter(byte id)
			: base(id)
		{
		}
	}
}
