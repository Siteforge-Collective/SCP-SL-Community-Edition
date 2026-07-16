namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicPointController : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		private enum RpcStateMsg
		{
			None = 0,
			PlacedByUser = 25,
			RemovedByUser = 26,
			DestroyedByDistance = 27
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.SpriteRenderer _mimicPointIcon;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Light _haloSource;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _iconOpacityOverDistance;

		[global::UnityEngine.SerializeField]
		private float _maxDistance;

		[global::UnityEngine.SerializeField]
		private float _focusOpacityReduction;

		private bool _active;

		private float _maxSqrDist;

		private global::RelativePositioning.RelativePosition _syncPos;

		private global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController.RpcStateMsg _syncMessage;

		private global::UnityEngine.Color _haloColor;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focus;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private const float CooldownDuration = 0.2f;

		private const int Scp939IconLayer = 22;

		private const int TeammatesIconLayer = 0;

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.Transform MimicPointTransform { get; private set; }

		public bool Active
		{
			get
			{
				return _active;
			}
			private set
			{
				if (value != _active)
				{
					_active = value;
					if (value)
					{
						UpdateMimicPoint();
						MainCameraController.OnUpdated += UpdateIcon;
						global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule.OnPositionUpdated += UpdateMimicPoint;
					}
					else
					{
						MimicPointTransform.localPosition = global::UnityEngine.Vector3.zero;
						_mimicPointIcon.gameObject.SetActive(value: false);
						MainCameraController.OnUpdated -= UpdateIcon;
						global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule.OnPositionUpdated -= UpdateMimicPoint;
					}
				}
			}
		}

		public event global::System.Action<global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation> OnMessageReceived;

		private bool TryGetIconLayer(out int layer)
		{
			layer = 0;
			if (!global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) && !ReferenceHub.TryGetLocalHub(out hub))
			{
				return false;
			}
			if (!hub.IsSCP())
			{
				return false;
			}
			if (hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp939.Scp939Role)
			{
				layer = 22;
			}
			return true;
		}

		private void UpdateMimicPoint()
		{
			global::UnityEngine.Vector3 position = _syncPos.Position;
			MimicPointTransform.position = position;
			UpdateIcon();
			if (global::Mirror.NetworkServer.active && !((base.ScpRole.FpcModule.Position - position).sqrMagnitude < _maxSqrDist))
			{
				_syncMessage = global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController.RpcStateMsg.DestroyedByDistance;
				ServerSendRpc(toAll: true);
			}
		}

		private void UpdateIcon()
		{
			if (!TryGetIconLayer(out var layer))
			{
				_mimicPointIcon.gameObject.SetActive(value: false);
				return;
			}
			global::UnityEngine.Vector3 position = MainCameraController.CurrentCamera.position;
			float time = global::UnityEngine.Vector3.Distance(position, _syncPos.Position);
			float num = _iconOpacityOverDistance.Evaluate(time);
			MimicPointTransform.LookAt(position);
			num -= _focus.State * _focusOpacityReduction;
			_mimicPointIcon.color = global::UnityEngine.Color.Lerp(global::UnityEngine.Color.clear, global::UnityEngine.Color.white, num);
			_haloSource.color = global::UnityEngine.Color.Lerp(global::UnityEngine.Color.clear, _haloColor, num);
			_mimicPointIcon.gameObject.layer = layer;
			_mimicPointIcon.gameObject.SetActive(value: true);
		}

		private void OnHubAdded(ReferenceHub hub)
		{
			if (global::Mirror.NetworkServer.active)
			{
				ServerSendRpc(hub);
			}
		}

		private void OnDestroy()
		{
			ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Remove(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(OnHubAdded));
		}

		protected override void Awake()
		{
			base.Awake();
			_maxSqrDist = _maxDistance * _maxDistance;
			_haloColor = _haloSource.color;
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focus);
			ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(OnHubAdded));
		}

		public override void ResetObject()
		{
			base.ResetObject();
			Active = false;
			_cooldown.Clear();
		}

		public void ClientToggle()
		{
			if (_cooldown.IsReady)
			{
				ClientSendCmd();
				_cooldown.Trigger(0.2f);
			}
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (Active)
			{
				_syncMessage = global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController.RpcStateMsg.RemovedByUser;
				Active = false;
			}
			else
			{
				_syncMessage = global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController.RpcStateMsg.PlacedByUser;
				_syncPos = new global::RelativePositioning.RelativePosition(base.ScpRole.FpcModule.Position);
				Active = true;
			}
			ServerSendRpc(toAll: true);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)_syncMessage);
			if (Active)
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _syncPos);
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_syncMessage = (global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController.RpcStateMsg)reader.ReadByte();
			switch (_syncMessage)
			{
			case global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController.RpcStateMsg.None:
				return;
			case global::PlayerRoles.PlayableScps.Scp939.Mimicry.MimicPointController.RpcStateMsg.PlacedByUser:
				_syncPos = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
				Active = true;
				break;
			default:
				Active = false;
				break;
			}
			this.OnMessageReceived?.Invoke((global::PlayerRoles.PlayableScps.Scp939.Scp939HudTranslation)_syncMessage);
		}
	}
}
