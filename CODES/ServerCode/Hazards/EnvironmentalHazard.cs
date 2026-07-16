namespace Hazards
{
	public abstract class EnvironmentalHazard : global::Mirror.NetworkBehaviour
	{
		public global::System.Collections.Generic.List<ReferenceHub> AffectedPlayers { get; } = new global::System.Collections.Generic.List<ReferenceHub>();

		[field: global::UnityEngine.SerializeField]
		public virtual float MaxDistance { get; set; }

		[field: global::UnityEngine.SerializeField]
		public virtual float MaxHeightDistance { get; set; }

		[field: global::UnityEngine.SerializeField]
		public virtual global::UnityEngine.Vector3 SourceOffset { get; set; }

		public virtual bool IsActive { get; } = true;

		public virtual global::UnityEngine.Vector3 SourcePosition
		{
			get
			{
				return base.transform.position + SourceOffset;
			}
			set
			{
				base.transform.position = value;
			}
		}

		public virtual void OnEnter(ReferenceHub player)
		{
			AffectedPlayers.Add(player);
		}

		public virtual void OnStay(ReferenceHub player)
		{
		}

		public virtual void OnExit(ReferenceHub player)
		{
			AffectedPlayers.Remove(player);
		}

		public virtual bool IsInArea(global::UnityEngine.Vector3 sourcePos, global::UnityEngine.Vector3 targetPos)
		{
			if (global::UnityEngine.Mathf.Abs(targetPos.y - sourcePos.y) > MaxHeightDistance)
			{
				return false;
			}
			return (sourcePos - targetPos).SqrMagnitudeIgnoreY() <= MaxDistance * MaxDistance;
		}

		protected virtual void UpdateTargets()
		{
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!(allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole))
				{
					continue;
				}
				bool flag = AffectedPlayers.Contains(allHub);
				if (IsInArea(SourcePosition, fpcRole.FpcModule.Position))
				{
					if (!flag)
					{
						OnEnter(allHub);
					}
					else
					{
						OnStay(allHub);
					}
				}
				else if (flag)
				{
					OnExit(allHub);
				}
			}
		}

		private void OnRoleChanged(ReferenceHub userHub, global::PlayerRoles.PlayerRoleBase prevRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (AffectedPlayers.Contains(userHub))
			{
				OnExit(userHub);
			}
		}

		protected virtual void Start()
		{
			if (global::Mirror.NetworkServer.active)
			{
				global::Mirror.NetworkServer.Spawn(base.gameObject);
				if (!global::GameCore.RoundStart.RoundStarted)
				{
					global::GameCore.Console.AddDebugLog("MAPGEN", "Spawning hazard: \"" + base.gameObject.name + "\"", MessageImportance.LessImportant, nospace: true);
				}
				global::PlayerRoles.PlayerRoleManager.OnRoleChanged += OnRoleChanged;
			}
		}

		protected virtual void Update()
		{
			if (global::Mirror.NetworkServer.active)
			{
				UpdateTargets();
			}
		}

		protected virtual void OnDestroy()
		{
			if (global::Mirror.NetworkServer.active)
			{
				global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
