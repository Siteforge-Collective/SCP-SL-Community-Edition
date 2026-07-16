using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Items.Jailbird
{
	public class JailbirdViewmodel : StandardAnimatedViemodel
	{
		[Serializable]
		private struct InspectPreset
		{
			public JailbirdWearState State;

			public AudioClip Sound;

			public float Speed;

			public int VariantId;
		}

		private static readonly Dictionary<ushort, double> LastUpdates;

		private static readonly Dictionary<ushort, JailbirdMessageType> LastRpcs;

		private static readonly HashSet<ushort> BrokenJailbirds;

		private static readonly int BrokenHash;

		private static readonly int LeftHandHash;

		private static readonly int ChargeLoadHash;

		private static readonly int ChargingHash;

		private static readonly int SkipPickupHash;

		private static readonly int AttackTriggerHash;

		private static readonly int InspectTriggerHash;

		private static readonly int InspectSpeedHash;

		private static readonly int InspectVariantHash;

		private static readonly int IdleTagHash;

		private static Dictionary<JailbirdWearState, InspectPreset> _presetsByWear;

		private static bool _alreadyPickedUp;

		private static bool _anyCollectionModified;

		private static bool _wasLeftHand;

		private const float FastModeThreshold = 1.5f;

		private const float InspectCooldown = 0.5f;

		[SerializeField]
		private JailbirdMaterialController _materialController;

		[SerializeField]
		private GameObject _particlesSmall;

		[SerializeField]
		private GameObject _particlesLarge;

		[SerializeField]
		private GameObject _particlesTrail;

		[SerializeField]
		private GameObject _particlesBroken;

		[SerializeField]
		private InspectPreset[] _inspectPresets;

		[SerializeField]
		private AudioClip _firstEquipSound;

		[SerializeField]
		private AudioClip _normalEquipSound;

		[SerializeField]
		private AudioClip _chargeLoadSound;

		[SerializeField]
		private AudioClip _chargingSound;

		[SerializeField]
		private AudioClip _swipeSoundLeft;

		[SerializeField]
		private AudioClip _swipeSoundRight;

		[SerializeField]
		private AudioClip _chargeHitSound;

		[SerializeField]
		private AudioClip _brokenSound;

		[SerializeField]
		private AudioSource _targetAudioSource;

		[SerializeField]
		private GameObject _inspectParticlesRoot;

		private double _nextInspect;

		private bool _wasCharging;

		public override void InitAny()
		{
		}

		internal override void OnEquipped()
		{
		}

		public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
		{
		}

		public override void InitLocal(ItemBase ib)
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void RpcReceived(ushort serial, JailbirdMessageType rpc)
		{
		}

		private void ProcessRpc(JailbirdMessageType rpc, float delay)
		{
		}

		private void PlaySound(AudioClip clip, float delay, bool stopPrev = true, float pitchRandom = 0f)
		{
		}

		private void SetBroken()
		{
		}

		private void OnCmdSent(JailbirdMessageType cmd)
		{
		}

		private void PlayAttackAnim(float delay)
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
