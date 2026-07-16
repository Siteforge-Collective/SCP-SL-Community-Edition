namespace PlayerRoles.PlayableScps.Subroutines
{
	public struct SubroutineMessage : global::Mirror.NetworkMessage
	{
		private readonly int _subroutineIndex;

		private readonly bool? _isConfirmation;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase _subroutine;

		private readonly ReferenceHub _target;

		private readonly global::PlayerRoles.RoleTypeId _role;

		private readonly global::Mirror.PooledNetworkReader _reader;

		public SubroutineMessage(global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase subroutine, bool isConfirmation)
		{
			_reader = null;
			_isConfirmation = isConfirmation;
			_subroutine = subroutine;
			_subroutineIndex = subroutine.SyncIndex;
			_role = subroutine.Role.RoleTypeId;
			subroutine.Role.TryGetOwner(out _target);
		}

		public SubroutineMessage(global::Mirror.NetworkReader reader)
		{
			_subroutine = null;
			_isConfirmation = null;
			_subroutineIndex = reader.ReadByte();
			if (_subroutineIndex == 0)
			{
				_reader = null;
				_target = null;
				_role = global::PlayerRoles.RoleTypeId.None;
				return;
			}
			_target = global::Utils.Networking.ReferenceHubReaderWriter.ReadReferenceHub(reader);
			_role = reader.ReadRoleType();
			int num = reader.ReadByte();
			if (num == 255)
			{
				num += global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			}
			_reader = global::Mirror.NetworkReaderPool.GetReader(reader.ReadBytesSegment(num));
		}

		public void Write(global::Mirror.NetworkWriter writer)
		{
			writer.WriteByte((byte)_subroutineIndex);
			if (_subroutineIndex != 0)
			{
				global::Utils.Networking.ReferenceHubReaderWriter.WriteReferenceHub(writer, _target);
				writer.WriteRoleType(_role);
				global::Mirror.PooledNetworkWriter writer2 = global::Mirror.NetworkWriterPool.GetWriter();
				if (_isConfirmation == true)
				{
					_subroutine.ServerWriteRpc(writer2);
				}
				else
				{
					_subroutine.ClientWriteCmd(writer2);
				}
				int num = writer2.Position;
				if (num > 65790)
				{
					num = 0;
				}
				writer.WriteByte((byte)global::System.Math.Min(num, 255));
				if (num >= 255)
				{
					global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, (ushort)(num - 255));
				}
				writer.WriteBytes(writer2.buffer, 0, num);
				writer2.Dispose();
			}
		}

		public void ServerApplyTrigger(global::Mirror.NetworkConnection conn)
		{
			if (_subroutineIndex != 0)
			{
				global::Mirror.NetworkIdentity identity = conn.identity;
				if (identity != null && ReferenceHub.TryGetHub(identity.gameObject, out var hub))
				{
					Apply(hub, server: true);
				}
				_reader.Dispose();
			}
		}

		public void ClientApplyConfirmation()
		{
			if (_subroutineIndex != 0)
			{
				if (_target != null)
				{
					Apply(_target, server: false);
				}
				_reader.Dispose();
			}
		}

		private void Apply(ReferenceHub hub, bool server)
		{
			if (!(hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Subroutines.ISubroutinedScpRole subroutinedScpRole) || hub.GetRoleId() != _role)
			{
				return;
			}
			int num = _subroutineIndex - 1;
			if (num < 0 || num >= subroutinedScpRole.SubroutineModule.AllSubroutines.Length)
			{
				return;
			}
			global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase scpSubroutineBase = subroutinedScpRole.SubroutineModule.AllSubroutines[num];
			if (server)
			{
				scpSubroutineBase.ServerProcessCmd(_reader);
				return;
			}
			try
			{
				scpSubroutineBase.ClientProcessRpc(_reader);
			}
			catch (global::System.Exception exception)
			{
				global::UnityEngine.Debug.LogException(exception);
			}
		}
	}
}
