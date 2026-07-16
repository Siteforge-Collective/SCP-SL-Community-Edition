namespace Hints
{
	public abstract class NetworkObject<TData> : global::Mirror.NetworkMessage
	{
		public abstract void Deserialize(global::Mirror.NetworkReader reader);

		public abstract void Serialize(global::Mirror.NetworkWriter writer);
	}
}
