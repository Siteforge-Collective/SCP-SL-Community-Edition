namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public struct FpcSyncData : global::System.IEquatable<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData>
	{
		private readonly global::PlayerRoles.FirstPersonControl.PlayerMovementState _state;

		private readonly global::RelativePositioning.RelativePosition _position;

		private readonly ushort _rotH;

		private readonly ushort _rotV;

		private readonly bool _bitMouseLook;

		private readonly bool _bitPosition;

		private readonly bool _bitCustom;

		public FpcSyncData(global::Mirror.NetworkReader reader)
		{
			Misc.ByteToBools(reader.ReadByte(), out var @bool, out var bool2, out var bool3, out var bool4, out var bool5, out _bitMouseLook, out _bitPosition, out _bitCustom);
			_state = (global::PlayerRoles.FirstPersonControl.PlayerMovementState)Misc.BoolsToByte(@bool, bool2, bool3, bool4, bool5);
			_position = (_bitPosition ? global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader) : new global::RelativePositioning.RelativePosition(0, 0, 0, 0));
			if (_bitMouseLook)
			{
				_rotH = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
				_rotV = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			}
			else
			{
				_rotH = 0;
				_rotV = 0;
			}
		}

		public FpcSyncData(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData prev, global::PlayerRoles.FirstPersonControl.PlayerMovementState state, bool bit, global::RelativePositioning.RelativePosition pos, global::PlayerRoles.FirstPersonControl.FpcMouseLook mLook)
		{
			_state = state;
			_bitCustom = bit;
			_position = pos;
			mLook.GetSyncValues(pos.WaypointId, out _rotH, out _rotV);
			_bitPosition = prev._position != _position;
			_bitMouseLook = _rotH != prev._rotH || _rotV != prev._rotV;
		}

		public void Write(global::Mirror.NetworkWriter writer)
		{
			Misc.ByteToBools((byte)_state, out var @bool, out var bool2, out var bool3, out var bool4, out var bool5, out var _, out var _, out var _);
			writer.WriteByte(Misc.BoolsToByte(@bool, bool2, bool3, bool4, bool5, _bitMouseLook, _bitPosition, _bitCustom));
			if (_bitPosition)
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _position);
			}
			if (_bitMouseLook)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, _rotH);
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, _rotV);
			}
		}

		public bool TryApply(ReferenceHub hub, out global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule module, out bool bit)
		{
			bit = _bitCustom;
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole) || !fpcRole.FpcModule.ModuleReady)
			{
				module = null;
				return false;
			}
			module = fpcRole.FpcModule;
			module.CurrentMovementState = _state;
			global::PlayerRoles.FirstPersonControl.FpcMotor motor = module.Motor;
			motor.MovementDetected = _bitPosition;
			if (_bitPosition)
			{
				motor.ReceivedPosition = _position;
			}
			if (_bitMouseLook)
			{
				module.MouseLook.ApplySyncValues(_rotH, _rotV);
			}
			return true;
		}

		public bool Equals(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData other)
		{
			if (_state == other._state && _position == other._position && _rotH == other._rotH)
			{
				return _rotV == other._rotV;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData fpcSyncData)
			{
				return fpcSyncData.Equals(this);
			}
			return false;
		}

		public static bool operator ==(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData left, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData left, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcSyncData right)
		{
			return !left.Equals(right);
		}

		public override int GetHashCode()
		{
			return (int)((((((uint)(_position.GetHashCode() * 397) ^ (uint)_state) * 397) ^ _rotH) * 397) ^ _rotV);
		}
	}
}
