public class Hitmarker : global::UnityEngine.MonoBehaviour
{
	public struct HitmarkerMessage : global::Mirror.NetworkMessage
	{
		public byte Size;

		public HitmarkerMessage(byte size)
		{
			Size = size;
		}
	}

	private const float MaxSize = 2.55f;

	public static void PlayHitmarker(float size)
	{
	}

	public static void SendHitmarker(ReferenceHub hub, float size)
	{
		size = global::UnityEngine.Mathf.Clamp(size, 0f, 2.55f);
		if (hub.isLocalPlayer)
		{
			PlayHitmarker(size);
		}
		else
		{
			global::PlayerRoles.Spectating.SpectatorNetworking.SendToSpectatorsOf(new Hitmarker.HitmarkerMessage((byte)global::UnityEngine.Mathf.RoundToInt(size / 2.55f * 255f)), hub, includeTarget: true);
		}
	}

	public static void SendHitmarker(global::Mirror.NetworkConnection conn, float size)
	{
		if (ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub))
		{
			SendHitmarker(hub, size);
		}
	}
}
