using System.Diagnostics;
using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
    public class AnimatedBelt : FunctionalFirearmPart
    {
        [SerializeField] private int _idleLayer;
        [SerializeField] private float _minimalCooldown;
        [SerializeField] private GameObject[] _bullets;

        private int _prevBullets;
        private bool _wasActive;
        private readonly Stopwatch _stopwatch = new();

        private int GetDisplayAmmo()
        {
            Firearm fa = Firearm;
            if (fa == null) return 0;

            return fa.Status.Ammo;
        }

        private void Start()
        {
            _wasActive = true;
            Firearm fa = Firearm;
            if (fa != null) fa.OnShotCalled += OnShot;

            _stopwatch.Start();
            Refresh(GetDisplayAmmo());
        }

        private void OnDestroy()
        {
            Firearm fa = Firearm;
            if (fa != null)
                fa.OnShotCalled -= OnShot;
        }

        private void OnEnable()
        {
            if (!_wasActive) return;
            Refresh(GetDisplayAmmo());
        }

        private void OnShot()
        {
            int curAmmo = GetDisplayAmmo();
            Refresh(curAmmo);
            _stopwatch.Restart();
        }

        private void Refresh(int curAmmo)
        {
            for (int i = 0; i < _bullets.Length; i++)
            {
                if (_bullets[i] != null)
                    _bullets[i].SetActive(i < curAmmo);
            }
            _prevBullets = curAmmo;
        }
    }
}