using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class GlowingBarrel : FunctionalFirearmPart
	{
		[Serializable]
		private struct AffectedMaterial
		{
			public Renderer[] Renderers;

			public int MaterialId;
		}

		[SerializeField]
		private AffectedMaterial[] _materialsToCopy;

		[Header("Worldmodel-only")]
		[Tooltip("Number of frames between material update, based on normalized temperature.")]
		[SerializeField]
		private AnimationCurve _updateCooldownOverTemperature;

		private bool _worldmodelMode;

		private FirearmWorldmodel _worldmodel;

		private ItemIdentifier _localIdentifier;

		private int _framesUntilNextUpdate;

		private readonly List<Material> _affectedMaterials;

		private static readonly int TemperatureHash;

		private void Start()
		{
		}

		private void Update()
		{
		}

		private float UpdateColor(ItemIdentifier id)
		{
			return 0f;
		}
	}
}
