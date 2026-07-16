using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.ThrowableProjectiles
{
	public class Scp2176Projectile : EffectGrenade
	{
		private const float LockdownDuration = 13f;

		private const float LockdownDisableValue = 0.1f;

		private const float PanicDuration = 5f;

		private const float ShatterVelocity = 8.5f;

		private const float ActivateVelocity = 6.5f;

		private const float DropSoundRange = 20f;

		private bool _hasTriggered;

		[SerializeField]
		private AudioClip _dropSound;

		[SyncVar]
		private bool _playedDropSound;

		public static event Action<RoomIdentifier> OnServerShattered;

		protected override void ProcessCollision(Collision collision)
		{
		}

		protected override void ServerFuseEnd()
		{
		}

		[ClientRpc]
		private void RpcMakeSound()
		{
		}

		public override void ServerActivate()
		{
		}

		public void ServerImmediatelyShatter()
		{
		}

		private void ServerShatter()
		{
		}

		private static bool TryFindTeslaAtRoom(RoomIdentifier rid, out TeslaGate gate)
		{
			gate = null;
			return false;
		}

		private void ServerOverloadTesla(RoomIdentifier rid, TeslaGate tg, IEnumerable<RoomLightController> lightControllers)
		{
		}

		private void ServerLockdown(IEnumerable<DoorVariant> doors)
		{
		}


		protected void UserCode_RpcMakeSound()
		{
		}

		protected static void InvokeUserCode_RpcMakeSound(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
		}

	}
}
