using PlayerRoles.PlayableScps.Scp079;
using UnityEngine;
using System.Collections.Generic;
using PlayerRoles.FirstPersonControl.Thirdperson;

namespace PlayerRoles.Spectating
{
    public class OverwatchNicknameGui : MonoBehaviour
    {
        [SerializeField] private RectTransform _fullscreenRect;
        [SerializeField] private RectTransform _template;

        private NicknameGuiDrawer _nicknameGuiDrawer;
        private Transform _cam;

        private void Start()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null) return;

            _nicknameGuiDrawer = gameObject.AddComponent<NicknameGuiDrawer>();
            _nicknameGuiDrawer.Init(mainCamera, _fullscreenRect, _template, false);
            _cam = mainCamera.transform;
        }

        private void LateUpdate()
        {
            if (this._nicknameGuiDrawer == null) return;

            this._nicknameGuiDrawer.ClearAll();

            ReferenceHub trackedHub;
            uint trackedNetId = 0;
            if (PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out trackedHub))
            {
                trackedNetId = trackedHub.netId;
            }

            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                if (hub.netId == trackedNetId)
                    continue;

                PlayerRoles.PlayerRoleBase currentRole = hub.roleManager.CurrentRole;
                if (currentRole == null)
                    continue;

                CharacterModel model = hub.gameObject.GetComponentInChildren<CharacterModel>();
                if (model == null)
                    continue;

                string displayName = hub.nicknameSync.DisplayName;
                Vector3 position = hub.transform.position;

                bool isScp = currentRole.Team == PlayerRoles.Team.SCPs;
                float zoom = 1f;
                this._nicknameGuiDrawer.TryDrawPlayer(
                    model,
                    displayName,
                    zoom,
                    position,
                    isScp
                );
            }
        }

        private void OnDestroy()
        {
            if (_nicknameGuiDrawer != null)
            {
                _nicknameGuiDrawer.ClearAll();
            }
        }
    }
}