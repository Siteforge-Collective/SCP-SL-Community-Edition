using InventorySystem.Items.Firearms;
using System.Diagnostics;

namespace InventorySystem.Items.Firearms.Modules
{
    public class EventBasedEquipper : IEquipperModule, IFirearmModuleBase
    {
        private bool _ready;

        private const float ServerTolerance = 0.1f;

        private readonly Firearm _firearm;

        private readonly Stopwatch _stopwatch;

        public bool Standby
        {
            get
            {
                if (!_firearm.IsLocalPlayer)
                    return true;

                if (_ready || _firearm.IsSpectated)
                    return true;

                if (!_stopwatch.IsRunning || _stopwatch.Elapsed.TotalSeconds < ServerTolerance)
                    return false;

                _stopwatch.Stop();
                _ready = true;
                FirearmLogger.Log("EQUIPPER",
                    $"serial={_firearm.ItemSerial} tolerance exceeded — now READY (elapsed={_stopwatch.Elapsed.TotalSeconds:F3}s)");
                return true;
            }
        }

        public EventBasedEquipper(Firearm firearm)
        {
            _ready = true;
            _firearm = firearm;
            _stopwatch = new Stopwatch();
        }

        public void OnEquipped()
        {
            FirearmLogger.Log("EQUIPPER",
                $"serial={_firearm.ItemSerial} OnEquipped — resetting ready=false");
            _ready = false;
            _stopwatch.Stop();
        }

        public void Equip()
        {
            if (_firearm.IsLocalPlayer)
            {
                FirearmLogger.Log("EQUIPPER",
                    $"serial={_firearm.ItemSerial} Equip() — local player, starting tolerance stopwatch");
                _stopwatch.Restart();
            }
            else
            {
                FirearmLogger.Log("EQUIPPER",
                    $"serial={_firearm.ItemSerial} Equip() — non-local, instantly ready");
                _ready = true;
            }
        }
    }
}