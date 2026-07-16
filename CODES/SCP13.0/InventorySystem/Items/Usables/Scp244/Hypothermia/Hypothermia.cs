using CustomPlayerEffects;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Searching;
using Mirror;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp244.Hypothermia
{
	public class Hypothermia : ParentEffectBase<HypothermiaSubEffectBase>, IWeaponModifierPlayerEffect, ISoundtrackMutingEffect, ISearchTimeModifier, IMovementSpeedModifier
	{
		public struct ForcedHypothermiaMessage : NetworkMessage
		{
			public bool IsForced;

			public float Exposure;

			public ReferenceHub PlayerHub;
		}

		private float _curExposure;

		private IWeaponModifierPlayerEffect _weaponModifier;

		private bool _isForced;

		private float _forcedExposure;

		private bool _wasForcedLastFrame;

		private const float IntensityRatio = 0.1f;

		public bool MuteSoundtrack { get; private set; }

		public bool ParamsActive { get; private set; }

		public bool MovementModifierActive => false;

		public float MovementSpeedMultiplier { get; private set; }

		public float MovementSpeedLimit { get; private set; }

		public float ProcessSearchTime(float val)
		{
			return 0f;
		}

		protected override void Update()
		{
		}

		private void UpdateSubEffect(HypothermiaSubEffectBase subEffect, float curExposure)
		{
		}

		private void UpdateExposure()
		{
		}

		public bool TryGetWeaponParam(AttachmentParam param, out float val)
		{
			val = default(float);
			return false;
		}

		private void CheckForceState()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void ClientReceiveForcedMessage(ForcedHypothermiaMessage message)
		{
		}
	}
}
