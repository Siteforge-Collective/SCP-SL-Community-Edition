using TMPro;
using UnityEngine;
using NorthwoodLib.Pools;
using System.Text;
using PlayerStatsSystem;
using PlayerRoles.FirstPersonControl;
using GameCore;

namespace PlayerRoles.Spectating
{
    public class OverwatchRole : SpectatorRole, IObfuscatedRole
    {
        [SerializeField]
        private TextMeshProUGUI _hudTemplate;

        private bool _hudSetup;

        private TextMeshProUGUI _hudInstance;

        public override RoleTypeId RoleTypeId => RoleTypeId.Overwatch; // Возвращает 21

        public override Color RoleColor => Color.cyan;

        public override bool ReadyToRespawn => false;

        public RoleTypeId GetRoleForUser(ReferenceHub receiver)
        {
            if (LastOwner == receiver ||
                PermissionsHandler.IsPermitted(receiver.serverRoles.Permissions, PlayerPermissions.GameplayData))
            {
                return RoleTypeId;
            }

            return base.RoleTypeId;
        }

        public override void DisableRole(RoleTypeId newRole)
        {
            base.DisableRole(newRole);

            if (_hudSetup && _hudInstance != null)
            {
                Destroy(_hudInstance.gameObject);
                _hudSetup = false;
            }
        }

        private void Update()
        {
            if (!SetupHud())
                return;

            ReferenceHub targetHub;
            if (PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out targetHub) && targetHub != null)
            {
                StringBuilder sb = StringBuilderPool.Shared.Rent();
                try
                {
                    sb.Append("<color=#008080>OVERWATCH MODE</color>");
                    sb.AppendFormat("\nPlayerID: {0}\nNickname: {1}", targetHub.PlayerId, targetHub.nicknameSync.MyNick);

                    if (targetHub.nicknameSync.HasCustomName)
                    {
                        sb.AppendFormat("\nDisplay name: {0}", targetHub.nicknameSync.DisplayName);
                    }

                    float speed = targetHub.GetVelocity().MagnitudeIgnoreY();
                    sb.AppendFormat("\nSpeed: {0:F4}", speed);

                    AdminFlagsStat adminFlags = targetHub.playerStats.GetModule<AdminFlagsStat>();
                    if (adminFlags.HasFlag(AdminFlags.Noclip)) // 1
                        sb.Append("\n<color=#DC143C>NOCLIP</color>");

                    if (adminFlags.HasFlag(AdminFlags.GodMode)) // 2
                        sb.Append("\n<color=#FFFF00>GOD MODE</color>");

                    if (adminFlags.HasFlag(AdminFlags.BypassMode)) // 4
                        sb.Append("\n<color=#33CC33>BYPASS MODE</color>");

                    // Получаем эффекты игрока
                    targetHub.playerEffectsController.GetAllSpectatorEffects(sb);

                    _hudInstance.text = sb.ToString();
                }
                catch (System.Exception ex)
                {
                    sb.Append("\n<color=#DC143C>EXCEPTION OCCURRED</color>");
                    Console.AddLog("Overwatch HUD Exception: " + ex.Message, Color.red);
                }
                finally
                {
                    StringBuilderPool.Shared.Return(sb);
                }
            }
            else if (_hudInstance != null)
            {
                _hudInstance.text = "<color=#008080>OVERWATCH MODE</color>\nNo target";
            }
        }

        private bool SetupHud()
        {
            if (LastOwner == null || !LastOwner.isLocalPlayer)
                return false;

            if (_hudSetup)
                return true;

            GameObject spectatorCanvas = GameObject.Find("Spectator Canvas");
            if (spectatorCanvas != null && _hudTemplate != null)
            {
                _hudInstance = Instantiate(_hudTemplate, spectatorCanvas.transform);

                RectTransform rt = _hudInstance.rectTransform;
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;
                rt.localRotation = Quaternion.identity;

                _hudSetup = true;
                return true;
            }

            return false;
        }
    }
}