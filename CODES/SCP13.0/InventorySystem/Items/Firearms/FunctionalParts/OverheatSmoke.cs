using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class OverheatSmoke : FunctionalFirearmPart
	{
		[SerializeField]
		private float _temperatureThreshold;

		[SerializeField]
		private ParticleSystem _particleSystem;

		private bool _emitting;

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void UpdateParticleSystem()
		{
		}
	}
}
