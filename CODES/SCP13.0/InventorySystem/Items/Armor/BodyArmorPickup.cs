using System.Collections.Generic;
using InventorySystem.Items.Pickups;
using UnityEngine;
using Mirror;

namespace InventorySystem.Items.Armor
{
    public class BodyArmorPickup : ItemPickupBase
    {
        private static readonly RigidbodyConstraints StartConstraints = (RigidbodyConstraints)80;
        private static readonly Quaternion StartRotation = Quaternion.Euler(0f, 0f, -90f);

        private readonly HashSet<ushort> _alreadyMovedPickups = new HashSet<ushort>();

        private float _remainingReleaseTime;
        private bool _released;
        private Rigidbody _rb;

        private bool IsAffected
        {
            get
            {
                if (!_released && NetworkServer.active)
                {
                    return PreviousOwner.IsSet;
                }
                return false;
            }
        }

        protected override void Start()
        {
            base.Start();

            if (IsAffected && PreviousOwner.Hub != null)
            {
                _remainingReleaseTime = 0.15f;
                if (PhysicsModule is PickupStandardPhysics standardPhysics)
                {
                    _rb = standardPhysics.Rb;
                    _rb.rotation = PreviousOwner.Hub.transform.rotation * StartRotation;
                    _rb.constraints = StartConstraints;
                }
            }
        }

        private void Update()
        {
            if (IsAffected && _rb != null)
            {
                if (Mathf.Abs(_rb.velocity.y) <= 0.1f)
                {
                    _remainingReleaseTime -= Time.deltaTime;
                    if (_remainingReleaseTime <= 0f)
                    {
                        _released = true;
                        _rb.constraints = RigidbodyConstraints.None;
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 9)
            {
                if (Vector3.Dot(Vector3.up, transform.right) <= -0.8f)
                {
                    if (other.transform.root.TryGetComponent<ItemPickupBase>(out var component))
                    {
                        if (component.Info.WeightKg <= 2.1f &&
                            _alreadyMovedPickups.Add(component.Info.Serial) &&
                            InventoryItemLoader.AvailableItems.TryGetValue(component.Info.ItemId, out var itemBase) &&
                            itemBase.Category != ItemCategory.Armor)
                        {
                            float distY = transform.position.y - component.transform.position.y;
                            component.transform.position += Vector3.up * (distY * 2f + 0.16f);
                        }
                    }
                }
            }
        }
    }
}