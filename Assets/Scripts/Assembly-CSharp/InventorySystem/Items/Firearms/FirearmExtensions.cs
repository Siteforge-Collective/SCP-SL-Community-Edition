using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using System;
using UnityEngine;

namespace InventorySystem.Items.Firearms
{
    public static class FirearmExtensions
    {
        public static Action<Firearm, byte, float> ServerSoundPlayed;

        public static void AnimForceUpdate(this Firearm fa)
        {
            if (fa.HasViewmodel && fa.ClientViewmodel != null)
            {
                fa.ClientViewmodel.AnimatorForceUpdate();
            }
        }

        public static void AnimSetInt(this Firearm fa, int hash, int i)
        {
            FirearmLogger.Log("ANIM",
                $"serial={fa.ItemSerial} SetInt hash={hash} val={i} " +
                $"srv={NetworkServer.active && fa.ServerSideAnimator != null} " +
                $"vm={fa.HasViewmodel && fa.ClientViewmodel != null}");
            if (NetworkServer.active && fa.ServerSideAnimator != null)
                fa.ServerSideAnimator.SetInteger(hash, i);

            if (fa.HasViewmodel && fa.ClientViewmodel != null)
                fa.ClientViewmodel.AnimatorSetInt(hash, i);
        }

        public static void AnimSetFloat(this Firearm fa, int hash, float f)
        {
            FirearmLogger.Log("ANIM",
                $"serial={fa.ItemSerial} SetFloat hash={hash} val={f:F3} " +
                $"srv={NetworkServer.active && fa.ServerSideAnimator != null} " +
                $"vm={fa.HasViewmodel && fa.ClientViewmodel != null}");
            if (NetworkServer.active && fa.ServerSideAnimator != null)
                fa.ServerSideAnimator.SetFloat(hash, f);

            if (fa.HasViewmodel && fa.ClientViewmodel != null)
                fa.ClientViewmodel.AnimatorSetFloat(hash, f);
        }

        public static void AnimSetBool(this Firearm fa, int hash, bool b)
        {
            FirearmLogger.Log("ANIM",
                $"serial={fa.ItemSerial} SetBool hash={hash} val={b} " +
                $"srv={NetworkServer.active && fa.ServerSideAnimator != null} " +
                $"vm={fa.HasViewmodel && fa.ClientViewmodel != null}");
            if (NetworkServer.active && fa.ServerSideAnimator != null)
                fa.ServerSideAnimator.SetBool(hash, b);

            if (fa.HasViewmodel && fa.ClientViewmodel != null)
                fa.ClientViewmodel.AnimatorSetBool(hash, b);
        }

        public static void AnimSetTrigger(this Firearm fa, int hash)
        {
            FirearmLogger.Log("ANIM",
                $"serial={fa.ItemSerial} SetTrigger hash={hash} " +
                $"srv={NetworkServer.active && fa.ServerSideAnimator != null} " +
                $"vm={fa.HasViewmodel && fa.ClientViewmodel != null}");
            if (NetworkServer.active && fa.ServerSideAnimator != null)
                fa.ServerSideAnimator.SetTrigger(hash);

            if (fa.HasViewmodel && fa.ClientViewmodel != null)
                fa.ClientViewmodel.AnimatorSetTrigger(hash);
        }

        public static void ServerSendAudioMessage(this Firearm firearm, byte clipId)
        {
            if (firearm == null || firearm.AudioClips == null || clipId >= firearm.AudioClips.Length)
            {
                FirearmLogger.Warn("SRV_AUDIO",
                    $"serial={firearm?.ItemSerial} clipId={clipId} — SKIP (null or out of range, clips={firearm?.AudioClips?.Length ?? -1})");
                return;
            }

            FirearmAudioClip clip = firearm.AudioClips[clipId];
            ReferenceHub owner = firearm.Owner;

            float maxDistance = clip.HasFlag(FirearmAudioFlags.ScaleDistance)
                ? clip.MaxDistance * AttachmentsUtils.AttachmentsValue(firearm, AttachmentParam.GunshotLoudnessMultiplier)
                : clip.MaxDistance;

            bool elevated = clip.HasFlag(FirearmAudioFlags.IsGunshot) && owner.transform.position.y > 900f;
            if (elevated)
                maxDistance *= 2.3f;

            FirearmLogger.Log("SRV_AUDIO",
                $"serial={firearm.ItemSerial} clipId={clipId} " +
                $"maxDist={maxDistance:F1} elevated={elevated} " +
                $"isGunshot={clip.HasFlag(FirearmAudioFlags.IsGunshot)}");

            float sqrMaxDistance = maxDistance * maxDistance;
            int sentCount = 0;

            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                if (hub == owner)
                    continue;

                if (hub.roleManager.CurrentRole is PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
                {
                    float sqrDist = (hub.transform.position - owner.transform.position).sqrMagnitude;
                    if (sqrDist > sqrMaxDistance)
                        continue;
                }

                float volume = Mathf.Clamp(maxDistance, 0f, 255f);
                byte volumeByte = (byte)Mathf.RoundToInt(volume);

                hub.networkIdentity.connectionToClient.Send(
                    new GunAudioMessage(owner, clipId, volumeByte, hub));
                sentCount++;
            }

            FirearmLogger.Log("SRV_AUDIO",
                $"serial={firearm.ItemSerial} clipId={clipId} — sent to {sentCount} clients");

            ServerSoundPlayed?.Invoke(firearm, clipId, maxDistance);
        }
    }
}