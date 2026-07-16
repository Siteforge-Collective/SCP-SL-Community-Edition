using System;
using System.Collections.Generic;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.SwayControllers;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class AnimatedFirearmViewmodel : AnimatedViewmodelBase
	{
		[Serializable]
		public struct ViewmodelAttachmentSettings
		{
			public GameObject[] ToggleableObjects;

			public AdsAttachmentSettings AdsSettings;

			public DualCamAttachmentSettings DualCamSettings;
		}

		[Serializable]
		public struct AdsAttachmentSettings
		{
			public float AdsFov;

			public Vector3 AdsPosition;

			public Vector3 AdsRotation;

			public float AdsScopeParameterAddition;

			public bool AdsOverride;
		}

		[Serializable]
		public struct DualCamAttachmentSettings
		{
			public GameObject TargetCameras;

			public Material DimmerMaterial;
		}

		private static readonly int AlbedoColor;

		private static readonly int EmissionColor;

		private static readonly int ScopeAds;

		private static readonly Dictionary<ushort, bool> SyncAdsStates;

		[SerializeField]
		private GoopSway.GoopSwaySettings _regularSwaySettings;

		[SerializeField]
		private GoopSway.GoopSwaySettings _adsSwaySettings;

		[SerializeField]
		private float _fov;

		[SerializeField]
		private Transform _cameraTrackerSource;

		[SerializeField]
		private Vector3 _cameraTrackerOffset;

		[SerializeField]
		private float _cameraTrackerIntensity;

		[SerializeField]
		private bool _randomizeShootAnims;

		public ViewmodelAttachmentSettings[] Attachments;

		private Firearm _fa;

		private AdsAttachmentSettings _combinedAds;

		private DualCamAttachmentSettings _combinedDual;

		private GoopSway _regularSway;

		private GoopSway _adsSway;

		private bool _useScopeParameter;

		private const float MaxReloadTime = 15f;

		public override float ViewmodelCameraFOV => 0f;

		public override IItemSwayController SwayController => null;

		public bool AudioMuted { get; private set; }

		internal override void OnEquipped()
		{
		}

		public override void InitAny()
		{
		}

		public override void InitSpectator(ReferenceHub ply, ItemIdentifier id, bool wasEquipped)
		{
		}

		private void OnDestroy()
		{
		}

		private void HandleEquipReload(RequestType rq)
		{
		}

		private void ProcessReceivedAudio(ReferenceHub rh, ItemType it, FirearmAudioClip fac)
		{
		}

		private void ProcessReceivedStatus(StatusMessage msg)
		{
		}

		private void ProcessRequestMessage(RequestMessage msg)
		{
		}

		public override void LateUpdate()
		{
		}

		protected virtual void OnShot()
		{
		}

		protected virtual void OnDryfired()
		{
		}

		public void UpdateAttachments()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
