using System.Collections.Generic;
using DeathAnimations;
using UnityEngine;

public class DisruptorDeathAnimation : DeathAnimation
{
	private const float DisolveSpeed = 1.5f;

	private const int TargetLayerMask = 1;

	private const float FlySpeed = 2f;

	private static readonly int DissolveHash;

	[SerializeField]
	private Material _templateMaterial;

	private List<Material> _materials;

	private bool _initialize;

	private float _timer;

	protected override void OnAnimationStarted()
	{
	}

	private bool ProcessRenderer(Renderer rend, List<Material> materials)
	{
		return false;
	}

	private void Update()
	{
	}
}
