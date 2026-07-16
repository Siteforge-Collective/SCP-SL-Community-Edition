namespace Hints
{
	public readonly struct HintMessage : global::Mirror.NetworkMessage
	{
		public readonly global::Hints.Hint Content;

		public HintMessage(global::Hints.Hint content)
		{
			Content = content;
		}
	}
}
