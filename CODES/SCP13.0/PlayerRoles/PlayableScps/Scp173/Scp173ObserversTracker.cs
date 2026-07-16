using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173ObserversTracker : StandardSubroutine<Scp173Role>
	{
		public delegate void ObserversChanged(int prev, int current);

		public readonly HashSet<ReferenceHub> Observers;

		private const float WidthMultiplier = 0.2f;

		[SerializeField]
		private float _modelWidth;

		[SerializeField]
		private float _maxViewDistance;

		[SerializeField]
		private Vector2[] _visibilityReferencePoints;

		private int _curObservers;

		private int _simulatedTargets;

		private float _simulatedStareTime;

		private readonly Stopwatch _simulatedStareSw;

		public int CurrentObservers
		{
			get
			{
				return 0;
			}
			private set
			{
			}
		}

		public bool IsObserved => false;

		public float SimulatedStare
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public event ObserversChanged OnObserversChanged
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

		private void Update()
		{
		}

		private void CheckRemovedPlayer(ReferenceHub ply)
		{
		}

		private int UpdateObserver(ReferenceHub targetHub)
		{
			return 0;
		}

		protected override void Awake()
		{
		}

		public bool IsObservedBy(ReferenceHub target, float widthMultiplier = 1f)
		{
			return false;
		}

		public void UpdateObservers()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}
	}
}
