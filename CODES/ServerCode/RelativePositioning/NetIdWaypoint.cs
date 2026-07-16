namespace RelativePositioning
{
	public class NetIdWaypoint : global::RelativePositioning.WaypointBase
	{
		[global::UnityEngine.SerializeField]
		private global::Mirror.NetworkIdentity _targetNetId;

		private global::UnityEngine.Vector3 _pos;

		private const byte Offset = 32;

		private static readonly global::System.Collections.Generic.HashSet<global::RelativePositioning.NetIdWaypoint> AllNetWaypoints = new global::System.Collections.Generic.HashSet<global::RelativePositioning.NetIdWaypoint>();

		private static bool _refreshNextFrame;

		private static bool _eventSet;

		protected override void Start()
		{
			base.Start();
			SetPosition();
			AllNetWaypoints.Add(this);
			_refreshNextFrame = true;
			if (!_eventSet)
			{
				global::MapGeneration.SeedSynchronizer.OnMapGenerated += delegate
				{
					_refreshNextFrame = true;
				};
				_eventSet = true;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			AllNetWaypoints.Remove(this);
		}

		protected override float SqrDistanceTo(global::UnityEngine.Vector3 pos)
		{
			return (pos - _pos).sqrMagnitude;
		}

		public override global::UnityEngine.Vector3 GetWorldspacePosition(global::UnityEngine.Vector3 relPosition)
		{
			return relPosition + _pos;
		}

		public override global::UnityEngine.Vector3 GetRelativePosition(global::UnityEngine.Vector3 worldPoint)
		{
			return worldPoint - _pos;
		}

		private void Update()
		{
			if (!_refreshNextFrame)
			{
				base.enabled = false;
				return;
			}
			byte b = 32;
			foreach (global::RelativePositioning.NetIdWaypoint item in global::System.Linq.Enumerable.OrderBy(AllNetWaypoints, (global::RelativePositioning.NetIdWaypoint x) => x._targetNetId.netId))
			{
				item.SetPosition();
				item.SetId(b);
				b++;
			}
			_refreshNextFrame = false;
		}

		private void Reset()
		{
			_targetNetId = GetComponent<global::Mirror.NetworkIdentity>();
		}

		private void SetPosition()
		{
			_pos = _targetNetId.transform.position;
		}
	}
}
