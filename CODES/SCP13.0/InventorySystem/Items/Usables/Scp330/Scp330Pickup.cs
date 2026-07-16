using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp330
{
	public class Scp330Pickup : CollisionDetectionPickup
	{
        [Serializable]
        private struct IndividualCandy
        {
            [SerializeField]
            private CandyKindID _kind;

            [SerializeField]
            private GameObject _candyObject;

            public void Refresh(CandyKindID exposed)
            {
                _candyObject.SetActive(exposed == _kind);
            }
        }

        public List<CandyKindID> StoredCandies = new List<CandyKindID>();

        [SyncVar]
		public CandyKindID ExposedCandy;

		[SerializeField]
		private IndividualCandy[] _candyTypes;

		private int _prevExposed;

        private void Update()
        {
            int exposedCandy = (int)ExposedCandy;
            if (_prevExposed != exposedCandy)
            {
                IndividualCandy[] candyTypes = _candyTypes;
                foreach (IndividualCandy individualCandy in candyTypes)
                {
                    individualCandy.Refresh(ExposedCandy);
                }
                _prevExposed = exposedCandy;
                if (NetworkServer.active && StoredCandies.Count == 0)
                {
                    DestroySelf();
                }
            }
        }
    }
}
