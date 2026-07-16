using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.HUDs
{
	public abstract class ScpHudBase : MonoBehaviour
	{
		private float _updateCounterTimer;

		private bool _eventAssigned;

		private bool _useCounter;

		public ReferenceHub Hub { get; private set; }

		[field: SerializeField]
		public TMP_Text TargetCounter { get; private set; }

		public event Action OnDestroyed
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

		protected virtual void ToggleHud(bool b)
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void UpdateCounter()
		{
		}

		internal virtual void OnDied()
		{
		}

		internal virtual void Init(ReferenceHub hub)
		{
		}
	}
}
