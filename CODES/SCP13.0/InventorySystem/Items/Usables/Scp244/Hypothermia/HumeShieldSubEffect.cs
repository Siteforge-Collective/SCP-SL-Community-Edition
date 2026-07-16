using System.Diagnostics;
using System.Runtime.InteropServices;
using CustomPlayerEffects;
using Mirror;
using PlayerRoles.PlayableScps.HumeShield;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class HumeShieldSubEffect : HypothermiaSubEffectBase, IHumeShieldBlocker
	{
		[StructLayout((LayoutKind)0, Size = 1)]
		public struct HumeBlockMsg : NetworkMessage
		{
		}

		[SerializeField]
		private AudioClip[] _freezeSounds;

		[SerializeField]
		private float _hsSustainTime;

		[SerializeField]
		private float _hsDecreaseStartTime;

		[SerializeField]
		private float _hsDecreaseAbsolute;

		[SerializeField]
		private float _hsDecreasePerExposure;

		private float _decreaseTimer;

		private static HumeShieldSubEffect _localEffect;

		private readonly Stopwatch _cooldownSw;

		private readonly Stopwatch _sustainSw;

		public override bool IsActive => false;

		public bool HumeShieldBlocked { get; private set; }

		[RuntimeInitializeOnLoadMethod]
		private static void Register()
		{
		}

		private static void OnMessageReceived(HumeBlockMsg msg)
		{
		}

		internal override void Init(StatusEffectBase mainEffect)
		{
		}

		internal override void UpdateEffect(float curExposure)
		{
		}

		private bool UpdateHumeShield(float expo)
		{
			return false;
		}

		private void ReceiveHumeBlockMessage()
		{
		}

		private bool TryGetController(out DynamicHumeShieldController ctrl)
		{
			ctrl = null;
			return false;
		}
	}
}
