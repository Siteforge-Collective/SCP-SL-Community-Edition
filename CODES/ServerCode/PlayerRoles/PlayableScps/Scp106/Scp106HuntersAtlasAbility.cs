namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106HuntersAtlasAbility : global::PlayerRoles.PlayableScps.Scp106.Scp106VigorAbilityBase, global::GameObjectPools.IPoolResettable
	{
		public const float CostPerMeter = 0.019f;

		private const ActionName SelectKey = ActionName.Shoot;

		private const int SyncAccuracy = 50;

		private const float SubmergeTime = 2.5f;

		private const float HeightOffset = 0.2f;

		private const float NormalMultiplier = 1.1f;

		private const float GroundDetectorHeight = 2f;

		private const float DissolvePercent = 0.5f;

		private const float MaxRetakeRange = 15f;

		private const float HeightTolerance = 400f;

		private const float DoorHeightTolerance = 50f;

		private const int OverlapSphereMaxDetections = 8;

		private const float MinVigor = 0.25f;

		private global::UnityEngine.Vector3 _syncPos;

		private global::MapGeneration.RoomIdentifier _syncRoom;

		private bool _submerged;

		private bool _dissolveAnim;

		private float _lastDissolveAmount;

		private float _estimatedCost;

		private static readonly global::UnityEngine.Collider[] DetectionsNonAlloc = new global::UnityEngine.Collider[8];

		private static readonly float DebugDuration = 0f;

		private static bool _maskSet;

		private static int _mask;

		private static int DetectionMask
		{
			get
			{
				if (!_maskSet)
				{
					_mask = global::UnityEngine.LayerMask.GetMask("Default", "Locker");
					_maskSet = true;
				}
				return _mask;
			}
		}

		protected override ActionName TargetKey => ActionName.Inventory;

		public override bool IsSubmerged => _submerged;

		protected override bool KeyPressable => base.Owner.isLocalPlayer;

		private void SetSubmerged(bool val)
		{
			if (_submerged != val)
			{
				_submerged = val;
				if (val)
				{
					_dissolveAnim = true;
					base.ScpRole.Sinkhole.TargetDuration = 2.5f;
				}
				if (global::Mirror.NetworkServer.active)
				{
					ServerSendRpc(toAll: true);
				}
			}
		}

		private void UpdateAny()
		{
			float normalizedState = base.ScpRole.Sinkhole.NormalizedState;
			if (_dissolveAnim && !_submerged && normalizedState < 0.5f)
			{
				_dissolveAnim = false;
			}
			if (base.Owner.isLocalPlayer || global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.Owner))
			{
				_lastDissolveAmount = (_dissolveAnim ? global::UnityEngine.Mathf.InverseLerp(0.5f, 1f, normalizedState) : 0f);
				global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.SetDissolveAnimation(_lastDissolveAmount);
			}
		}

		private void UpdateClientside()
		{
			global::PlayerRoles.PlayableScps.Scp106.Scp106Minimap singleton = global::PlayerRoles.PlayableScps.Scp106.Scp106Minimap.Singleton;
			if (singleton == null)
			{
				return;
			}
			if (base.ScpRole.Sinkhole.NormalizedState != 0f || !IsKeyHeld || !base.ScpRole.FpcModule.IsGrounded)
			{
				singleton.IsVisible = false;
				return;
			}
			if (!base.ScpRole.Sinkhole.Cooldown.IsReady)
			{
				singleton.IsVisible = false;
				global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.PlayFlash(vigor: false);
				return;
			}
			if (base.Vigor.VigorAmount < 0.25f)
			{
				singleton.IsVisible = false;
				global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.PlayFlash(vigor: true);
				return;
			}
			singleton.IsVisible = true;
			if (global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement.AnyHighlighted && global::UnityEngine.Input.GetKey(NewInput.GetKey(ActionName.Shoot)))
			{
				_syncPos = singleton.LastWorldPos;
				_syncRoom = global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement.LastHighlighted.Room;
				ClientSendCmd();
			}
		}

		private void UpdateServerside()
		{
			if (_submerged && !(base.ScpRole.Sinkhole.NormalizedState < 1f))
			{
				global::UnityEngine.Vector3 safePosition = GetSafePosition();
				float num = (safePosition - base.ScpRole.FpcModule.Position).MagnitudeIgnoreY();
				base.Vigor.VigorAmount -= global::UnityEngine.Mathf.Min(_estimatedCost, num * 0.019f);
				base.ScpRole.FpcModule.ServerOverridePosition(safePosition, global::UnityEngine.Vector3.zero);
				SetSubmerged(val: false);
			}
		}

		private global::UnityEngine.Vector3 GetSafePosition()
		{
			global::UnityEngine.Vector3 result = base.ScpRole.FpcModule.Position;
			float num = float.MaxValue;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant allDoor in global::Interactables.Interobjects.DoorUtils.DoorVariant.AllDoors)
			{
				if (!(allDoor is global::Interactables.Interobjects.DoorUtils.IScp106PassableDoor scp106PassableDoor) || !scp106PassableDoor.IsScp106Passable || !allDoor.Rooms.Contains(_syncRoom))
				{
					continue;
				}
				global::UnityEngine.Vector3 position = allDoor.transform.position;
				if (!(global::UnityEngine.Mathf.Abs(position.y - _syncPos.y) > 50f))
				{
					global::UnityEngine.Vector3 vector = ClosestDoorPosition(position);
					float num2 = (vector - _syncPos).SqrMagnitudeIgnoreY();
					if (DebugDuration > 0f)
					{
						allDoor.name = $"Debug door, disSqr={num2}";
					}
					if (!(num2 > num))
					{
						num = num2;
						result = vector;
					}
				}
			}
			return result;
		}

		private global::UnityEngine.Vector3 ClosestDoorPosition(global::UnityEngine.Vector3 doorPos)
		{
			global::UnityEngine.Vector3 vector = _syncPos - doorPos;
			global::UnityEngine.Vector3 dir = new global::UnityEngine.Vector3(vector.x, 0f, vector.z);
			float num = dir.magnitude;
			if (num > 0f)
			{
				dir /= num;
			}
			float radius = base.ScpRole.FpcModule.CharController.radius;
			float height = base.ScpRole.FpcModule.CharController.height;
			global::UnityEngine.Vector3 origin = doorPos + global::UnityEngine.Vector3.up * (0.2f + radius);
			global::UnityEngine.Color debugColor = ((DebugDuration > 0f) ? global::UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.4f, 0.8f) : global::UnityEngine.Color.clear);
			do
			{
				if (TrySphereCast(debugColor, origin, dir, radius, height, num, out var pos))
				{
					return pos;
				}
				num = global::UnityEngine.Mathf.Min(15f, num - radius);
			}
			while (!(num < radius));
			return doorPos + global::UnityEngine.Vector3.up * height;
		}

		private bool TrySphereCast(global::UnityEngine.Color debugColor, global::UnityEngine.Vector3 origin, global::UnityEngine.Vector3 dir, float radius, float height, float maxDis, out global::UnityEngine.Vector3 pos)
		{
			global::UnityEngine.Debug.DrawRay(origin, dir, debugColor, DebugDuration);
			if (global::UnityEngine.Physics.SphereCast(origin, radius, dir, out var hitInfo, maxDis + radius, DetectionMask))
			{
				hitInfo.point += 1.1f * radius * hitInfo.normal;
			}
			else
			{
				hitInfo.point = origin + dir * maxDis;
			}
			pos = hitInfo.point;
			if (DebugDuration > 0f)
			{
				DebugHitPoint(hitInfo.point, debugColor);
			}
			if (!global::UnityEngine.Physics.Raycast(pos + global::UnityEngine.Vector3.up * 0.2f, global::UnityEngine.Vector3.down, out var hitInfo2, 2f, DetectionMask))
			{
				return false;
			}
			int num = global::UnityEngine.Physics.OverlapSphereNonAlloc(hitInfo2.point, radius * 2f, DetectionsNonAlloc);
			for (int i = 0; i < num; i++)
			{
				if (DetectionsNonAlloc[i].TryGetComponent<TeslaGate>(out var _))
				{
					return false;
				}
			}
			pos = hitInfo2.point + global::UnityEngine.Vector3.up * (0.2f + radius);
			if (global::UnityEngine.Physics.CheckCapsule(pos, pos + global::UnityEngine.Vector3.up * (height - radius - 0.2f), radius, DetectionMask))
			{
				return false;
			}
			pos = hitInfo2.point + global::UnityEngine.Vector3.up * height;
			return true;
		}

		private void DebugHitPoint(global::UnityEngine.Vector3 point, global::UnityEngine.Color debugColor)
		{
			global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[6]
			{
				global::UnityEngine.Vector3.up,
				global::UnityEngine.Vector3.down,
				global::UnityEngine.Vector3.left,
				global::UnityEngine.Vector3.right,
				global::UnityEngine.Vector3.forward,
				global::UnityEngine.Vector3.back
			};
			foreach (global::UnityEngine.Vector3 vector in array)
			{
				global::UnityEngine.Debug.DrawLine(point, point + vector * 0.1f, debugColor, DebugDuration);
			}
		}

		protected override void Update()
		{
			base.Update();
			UpdateAny();
			if (base.Owner.isLocalPlayer)
			{
				UpdateClientside();
			}
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServerside();
			}
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::UnityEngine.Vector3 position = _syncRoom.transform.position;
			global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, new global::RelativePositioning.RelativePosition(position));
			global::UnityEngine.Vector3Int vector3Int = global::UnityEngine.Vector3Int.RoundToInt((_syncPos - position) * 50f);
			global::Mirror.NetworkWriterExtensions.WriteInt16(writer, (short)vector3Int.x);
			global::Mirror.NetworkWriterExtensions.WriteInt16(writer, (short)vector3Int.z);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (base.ScpRole.Sinkhole.NormalizedState > 0f || !base.ScpRole.Sinkhole.Cooldown.IsReady)
			{
				return;
			}
			global::UnityEngine.Vector3 position = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader).Position;
			_syncRoom = global::MapGeneration.RoomIdUtils.RoomAtPosition(position);
			global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(global::Mirror.NetworkReaderExtensions.ReadInt16(reader), 0f, global::Mirror.NetworkReaderExtensions.ReadInt16(reader));
			_syncPos = position + vector / 50f;
			if (_syncRoom == null)
			{
				return;
			}
			global::UnityEngine.Vector3 position2 = base.ScpRole.FpcModule.Position;
			if (!(global::UnityEngine.Mathf.Abs(position2.y - _syncPos.y) > 400f))
			{
				float num = (position2 - _syncPos).MagnitudeIgnoreY() * 0.019f;
				if (!(num > base.Vigor.VigorAmount))
				{
					_estimatedCost = num;
					SetSubmerged(val: true);
				}
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, IsSubmerged);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (!global::Mirror.NetworkServer.active)
			{
				SetSubmerged(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_submerged = false;
			_dissolveAnim = false;
			if (_lastDissolveAmount > 0f)
			{
				_lastDissolveAmount = 0f;
				global::PlayerRoles.PlayableScps.Scp106.Scp106Hud.SetDissolveAnimation(0f);
			}
		}
	}
}
