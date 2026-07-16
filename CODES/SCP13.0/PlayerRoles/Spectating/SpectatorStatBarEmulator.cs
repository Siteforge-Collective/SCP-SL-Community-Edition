using System;
using PlayerStatsSystem;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.Spectating
{
    public class SpectatorStatBarEmulator : MonoBehaviour
    {
        private GameObject _statsRoot;

        private static readonly Type[] RecolorableModules = new Type[]
        {
            typeof(HealthStat)
        };

        private void Start()
        {
            SpectatorTargetTracker.OnTargetChanged += OnTargetChanged;

            if (UserMainInterface.Singleton != null)
            {
                _statsRoot = UserMainInterface.Singleton.PlyStats;
            }

            OnTargetChanged();
        }

        private void OnDestroy()
        {
            SpectatorTargetTracker.OnTargetChanged -= OnTargetChanged;
        }

        private void OnTargetChanged()
        {
            if (SpectatorTargetTracker.TryGetTrackedPlayer(out ReferenceHub targetHub))
            {
                if (targetHub.roleManager.CurrentRole != null)
                {
                    if (_statsRoot != null)
                        _statsRoot.SetActive(true);

                    StatSliderManager.TryForEach(slider => OnTargetSwitched(targetHub, slider));
                    return;
                }
            }

            if (_statsRoot != null)
            {
                _statsRoot.SetActive(false);
            }
        }

        private static void OnTargetSwitched(ReferenceHub hub, StatSlider slider)
        {
            slider.ForceUpdate();

            if (slider.TryGetComponent<StatusBar>(out var statusBar))
            {
                statusBar.UpdateBar(true);
            }

            if (slider.TryGetModule(out StatBase module))
            {
                if (RecolorableModules.Contains(module.GetType()))
                {
                    Color roleColor = hub.roleManager.CurrentRole.RoleColor;
                    slider.gameObject.ForEachComponentInChildren<Graphic>(graphic =>
                        RecolorRetainAlpha(graphic, roleColor), true);
                }
            }
        }

        private static void RecolorRetainAlpha(Graphic target, Color toApply)
        {
            Color current = target.color;
            target.color = new Color(toApply.r, toApply.g, toApply.b, current.a);
        }
    }
}