using Decals;
using InventorySystem.Items.Firearms.BasicMessages;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Firearms.Modules
{
	public abstract class StandardHitregBase : IHitregModule, IFirearmModuleBase
	{
		public static readonly LayerMask HitregMask;

		private const float MinDot = 0.5f;

		private const float MaxHeightDiff = 50f;

		public bool Standby => false;

		protected NetworkConnection Conn => null;

		protected abstract Firearm Firearm { get; set; }

		protected abstract ReferenceHub Hub { get; set; }

		protected abstract DecalPoolType BulletHoleDecal { get; }

		public static bool DebugMode { get; internal set; }

		protected uint PrimaryTargetNetId { get; private set; }

		private void SetHitboxes(ReferenceHub target, bool state)
		{
		}

		protected void SendDebug(string msg)
		{
		}

		public bool ClientCalculateHit(out ShotMessage message)
		{
			message = default(ShotMessage);
			return false;
		}

		public void ServerProcessShot(ShotMessage message)
		{
		}

		protected void ShowHitIndicator(uint netId, float damage, Vector3 origin)
		{
		}

		protected void PlaceBulletholeDecal(Ray ray, RaycastHit hit)
		{
		}

		protected void PlaceBloodDecal(Ray ray, RaycastHit hit, IDestructible target)
		{
		}

		protected abstract void ServerPerformShot(Ray ray);

		private void SendDecalMessage(Ray ray, RaycastHit hit, DecalPoolType decal)
		{
		}
	}
}
