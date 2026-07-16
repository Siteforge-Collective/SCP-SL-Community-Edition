using Mirror;
using PlayerStatsSystem;
using UnityEngine;

public class Hitmarker : MonoBehaviour
{
	public struct HitmarkerMessage : NetworkMessage
	{
		public byte Size;

		public bool Audio;

		public HitmarkerMessage(byte size, bool playAudio = true)
		{
			Size = 0;
			Audio = false;
		}
	}

	[SerializeField]
	private AnimationCurve _sizeOverTime;

	[SerializeField]
	private AnimationCurve _opacityOverTime;

	[SerializeField]
	private CanvasRenderer _targetImage;

	[SerializeField]
	private float _animationTime;

	[SerializeField]
	private AudioClip[] _hitMarkerSounds;

	private float _timer;

	private float _targetSize;

	private static Hitmarker _singleton;

	private const float MaxSize = 2.55f;

	[RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
	}

	private static void HitmarkerMsgReceived(HitmarkerMessage msg)
	{
	}

	private void Awake()
	{
	}

	private void Update()
	{
	}

	public static void PlayHitmarker(float size, bool playAudio = true)
	{
	}

	public static void SendHitmarkerDirectly(ReferenceHub hub, float size, bool playAudio = true)
	{
	}

	public static void SendHitmarkerDirectly(NetworkConnection conn, float size)
	{
	}

	public static void SendHitmarkerConditionally(float size, AttackerDamageHandler adh, ReferenceHub victim)
	{
	}

	public static bool CheckHitmarkerPerms(AttackerDamageHandler adh, ReferenceHub victim)
	{
		return false;
	}
}
