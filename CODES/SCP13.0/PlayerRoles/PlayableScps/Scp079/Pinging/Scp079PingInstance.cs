using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Pinging
{
	public class Scp079PingInstance : MonoBehaviour
	{
		public static readonly HashSet<Scp079PingInstance> Instances;

		[SerializeField]
		private float _destroyTime;

		[SerializeField]
		private Transform _icon;

		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[SerializeField]
		private AnimationCurve _sizeOverDistance;

		[SerializeField]
		private float _distanceCap;

		[SerializeField]
		private Renderer[] _renderers;

		[SerializeField]
		private AudioSource _src;

		private Vector3 _startPos;

		private bool _wasVisible;

		private const float MaxRangeSqr = 5000f;

		public Sprite IconSprite
		{
			set
			{
			}
		}

		private bool IsVisible => false;

		public static event Action<Scp079PingInstance> OnSpawned
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void OnValidate()
		{
		}

		private void UpdateVisibility()
		{
		}
	}
}
