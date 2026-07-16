using InventorySystem.Items.Firearms.Attachments.Components;
using UnityEngine;

namespace InventorySystem.Items.Firearms.FunctionalParts
{
	public class ReflexSight : FunctionalFirearmPart
	{
		private static readonly int HashTexture;

		private static readonly int HashColor;

		private static readonly int HashSize;

		[SerializeField]
		private Renderer _targetRenderer;

		private Material _mat;

		private ReflexSightAttachment _sightAtt;

		private Color _targetColor;

		private float? _prevAds;

		private void Start()
		{
		}

		private void LateUpdate()
		{
		}

		private void UpdateValues()
		{
		}

		private void SetMaterial(Texture texture, float size, Color color)
		{
		}
	}
}
