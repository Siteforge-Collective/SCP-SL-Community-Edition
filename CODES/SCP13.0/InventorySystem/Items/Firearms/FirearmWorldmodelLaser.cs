using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace InventorySystem.Items.Firearms
{
	public class FirearmWorldmodelLaser : MonoBehaviour
	{
		private static readonly CachedLayerMask Mask;

		private const float LaserPickupRange = 8f;

		private const float LaserThirdpersonRange = 30f;

		private const float FadeDistance = 5f;

		private const float TransitionDistance = 9f;

		private const float CameraFwdOffset = 0.33f;

		private const int FadePower = 3;

		private FirearmThirdpersonItem _thirdperson;

		private Transform _originTransform;

		private Transform _decalTransform;

		private bool _pickupMode;

		private RaycastHit _lastHit;

		private float _range;

		[SerializeField]
		private DecalProjector _decal;

		private void Awake()
		{
		}

		private void LateUpdate()
		{
		}

		private bool UpdatePickup()
		{
			return false;
		}

		private bool UpdateThirdperson()
		{
			return false;
		}

		private bool TryRaycast(Vector3 pos, Vector3 fwd, out RaycastHit hit)
		{
			hit = default(RaycastHit);
			return false;
		}
	}
}
