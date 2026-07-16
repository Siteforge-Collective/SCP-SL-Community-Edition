using UnityEngine;

namespace InventorySystem.Items.Firearms
{
	public class AutomaticFirearmAnimatorEvents : FirearmAnimatorEventsBase
	{
		private float _curGripStatus;

		private float _gripMoveSpeed;

		[Tooltip("Defines if grip blend Animator float is being set regardless of a grip attachment being installed.")]
		[Header("Clientside viewmodel settings (ignore on server-side instance)")]
		[SerializeField]
		private bool _alwaysProcessGrip;

		[Tooltip("If _gripAttachmentIds is false, this contains list of attachments that are valid grip blend triggers.")]
		[SerializeField]
		private int[] _gripAttachmentIds;

		[Tooltip("Layer at which animations affected by grips are played.")]
		[SerializeField]
		private int _stateMultiplierLayer;

		private float _prevAds;

		private bool _resetEventAssigned;

		private bool AnyGripInstalled => false;

		private void InsertMagazine()
		{
		}

		private void RemoveMagazine()
		{
		}

		private void RemoveMagazineOpenBolt()
		{
		}

		private void UseChargingHandle()
		{
		}

		private void UnloadChamberedBullet()
		{
		}

		private void MarkAsEquipped()
		{
		}

		private void SetGripBlendSpeed(float speed)
		{
		}

		private void OnEnable()
		{
		}

		private void Update()
		{
		}

		private void UpdateGrip()
		{
		}

		private void ResetGrip()
		{
		}

		private void RefreshAnim(bool force)
		{
		}
	}
}
