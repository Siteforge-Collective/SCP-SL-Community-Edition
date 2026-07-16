namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Vigor : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::GameObjectPools.IPoolSpawnable
	{
		private bool _force;

		private int _lastSentAmount;

		private float _vigor;

		private const float SyncAccuracy = 120f;

		private const float AbsMoveSpeed = 0.03f;

		private const float LerpMoveSpeed = 2.7f;

		private const float StartAmount = 0f;

		public float VigorAmount
		{
			get
			{
				return global::UnityEngine.Mathf.Clamp01(_vigor);
			}
			set
			{
				_vigor = value;
			}
		}

		public float DisplayedVigor { get; private set; }

		private void Update()
		{
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServerside();
			}
			UpdateClientside();
		}

		private void UpdateClientside()
		{
			if (_force)
			{
				_force = false;
				DisplayedVigor = VigorAmount;
			}
			else
			{
				DisplayedVigor = global::UnityEngine.Mathf.MoveTowards(DisplayedVigor, VigorAmount, global::UnityEngine.Time.deltaTime * 0.03f);
				DisplayedVigor = global::UnityEngine.Mathf.Lerp(DisplayedVigor, VigorAmount, global::UnityEngine.Time.deltaTime * 2.7f);
			}
		}

		private void UpdateServerside()
		{
			if (!base.Role.TryGetOwner(out var owner))
			{
				return;
			}
			int num = global::UnityEngine.Mathf.FloorToInt(VigorAmount * 120f);
			if (num != _lastSentAmount)
			{
				_lastSentAmount = num;
				ServerSendRpc((ReferenceHub x) => x == owner || x.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole);
			}
			else
			{
				_force = false;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			global::PlayerRoles.PlayerRoleManager.OnRoleChanged += delegate(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase prev, global::PlayerRoles.PlayerRoleBase cur)
			{
				if (global::Mirror.NetworkServer.active && cur is global::PlayerRoles.Spectating.SpectatorRole)
				{
					ServerSendRpc(hub);
				}
			};
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			int num = _lastSentAmount + 1;
			global::Mirror.NetworkWriterExtensions.WriteSByte(writer, (sbyte)(_force ? (-num) : num));
			_force = false;
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			if (!global::Mirror.NetworkServer.active)
			{
				sbyte b = global::Mirror.NetworkReaderExtensions.ReadSByte(reader);
				_force = b < 0;
				VigorAmount = (float)global::UnityEngine.Mathf.Abs(b) / 120f;
			}
		}

		public void SpawnObject()
		{
			VigorAmount = 0f;
			DisplayedVigor = 0f;
			_lastSentAmount = -1;
			_force = true;
		}
	}
}
