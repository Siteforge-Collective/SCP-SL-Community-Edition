using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class ShootingParticleEffect : FunctionalFirearmPart
	{
		[SerializeField]
		private ParticleSystem[] _targetParticleSystems;

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnShot()
		{
		}
	}
}
