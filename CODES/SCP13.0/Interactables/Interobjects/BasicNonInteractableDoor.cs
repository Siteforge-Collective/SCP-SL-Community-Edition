using System.Runtime.InteropServices;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using UnityEngine;

namespace Interactables.Interobjects
{
    public class BasicNonInteractableDoor : BasicDoor, INonInteractableDoor, IScp106PassableDoor
    {
        [SerializeField]
        private bool _ignoreLockdowns;

        [SerializeField]
        private bool _ignoreRemoteAdmin;

        [SerializeField]
        [SyncVar]
        private bool _blockScp106;

        public bool IgnoreLockdowns => _ignoreLockdowns;

        public bool IgnoreRemoteAdmin => _ignoreRemoteAdmin;

        public bool IsScp106Passable
        {
            get
            {
                return !_blockScp106;
            }
            set
            {
                _blockScp106 = !value;
            }
        }
    }
}
