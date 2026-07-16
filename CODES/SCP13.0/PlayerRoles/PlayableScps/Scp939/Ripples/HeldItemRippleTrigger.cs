using System.Collections.Generic;
using InventorySystem.Items.Usables;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939.Ripples
{
	public class HeldItemRippleTrigger : RippleTriggerBase
	{
		[SerializeField]
		private float _cooldown;

		private readonly AbilityCooldown _cd;

		private readonly Dictionary<ushort, bool> _usableItemStates;

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		private void ProcessUsableMessage(StatusMessage msg)
		{
		}

		private void Update()
		{
		}

		private void ProcessPlayer(ReferenceHub hub, HumanRole role)
		{
		}

		private bool ProcessMicro(ushort serial)
		{
			return false;
		}
	}
}
