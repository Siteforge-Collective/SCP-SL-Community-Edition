namespace InventorySystem.Items.Firearms
{
	public static class FirearmExtensions
	{
		public static global::System.Action<global::InventorySystem.Items.Firearms.Firearm, byte, float> ServerSoundPlayed = delegate
		{
		};

		public static void AnimForceUpdate(this global::InventorySystem.Items.Firearms.Firearm fa)
		{
		}

		public static void AnimSetInt(this global::InventorySystem.Items.Firearms.Firearm fa, int hash, int i)
		{
			if (global::Mirror.NetworkServer.active)
			{
				fa.ServerSideAnimator.SetInteger(hash, i);
			}
		}

		public static void AnimSetFloat(this global::InventorySystem.Items.Firearms.Firearm fa, int hash, float f)
		{
			if (global::Mirror.NetworkServer.active)
			{
				fa.ServerSideAnimator.SetFloat(hash, f);
			}
		}

		public static void AnimSetBool(this global::InventorySystem.Items.Firearms.Firearm fa, int hash, bool b)
		{
			if (global::Mirror.NetworkServer.active)
			{
				fa.ServerSideAnimator.SetBool(hash, b);
			}
		}

		public static void AnimSetTrigger(this global::InventorySystem.Items.Firearms.Firearm fa, int hash)
		{
			if (global::Mirror.NetworkServer.active)
			{
				fa.ServerSideAnimator.SetTrigger(hash);
			}
		}

		public static void ServerSendAudioMessage(this global::InventorySystem.Items.Firearms.Firearm firearm, byte clipId)
		{
			global::InventorySystem.Items.Firearms.FirearmAudioClip firearmAudioClip = firearm.AudioClips[clipId];
			ReferenceHub owner = firearm.Owner;
			float num = (firearmAudioClip.HasFlag(global::InventorySystem.Items.Firearms.FirearmAudioFlags.ScaleDistance) ? (firearmAudioClip.MaxDistance * global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.GunshotLoudnessMultiplier)) : firearmAudioClip.MaxDistance);
			if (firearmAudioClip.HasFlag(global::InventorySystem.Items.Firearms.FirearmAudioFlags.IsGunshot) && owner.transform.position.y > 900f)
			{
				num *= 2.3f;
			}
			float num2 = num * num;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub == firearm.Owner) && (!(allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole) || !((allHub.transform.position - owner.transform.position).sqrMagnitude > num2)))
				{
					allHub.networkIdentity.connectionToClient.Send(new global::InventorySystem.Items.Firearms.GunAudioMessage(owner, clipId, (byte)global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.Clamp(num, 0f, 255f)), allHub));
				}
			}
			ServerSoundPlayed?.Invoke(firearm, clipId, num);
		}
	}
}
