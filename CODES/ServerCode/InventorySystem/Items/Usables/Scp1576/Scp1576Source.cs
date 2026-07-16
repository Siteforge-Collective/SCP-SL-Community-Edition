namespace InventorySystem.Items.Usables.Scp1576
{
	public class Scp1576Source : global::UnityEngine.MonoBehaviour
	{
		public static global::System.Action<global::InventorySystem.Items.Usables.Scp1576.Scp1576Source> OnRemoved;

		public static global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Usables.Scp1576.Scp1576Source> Instances = new global::System.Collections.Generic.HashSet<global::InventorySystem.Items.Usables.Scp1576.Scp1576Source>();

		private global::UnityEngine.Transform _cachedTransform;

		private bool _transformCacheSet;

		private bool _positionUpToDate;

		private global::UnityEngine.Vector3 _lastPos;

		public global::UnityEngine.Vector3 Position
		{
			get
			{
				if (!_positionUpToDate)
				{
					_lastPos = CachedTransform.position;
					_positionUpToDate = true;
				}
				return _lastPos;
			}
		}

		[field: global::UnityEngine.SerializeField]
		public bool HideGlobalIndicator { get; private set; }

		private global::UnityEngine.Transform CachedTransform
		{
			get
			{
				if (!_transformCacheSet)
				{
					_cachedTransform = base.transform;
					_transformCacheSet = true;
				}
				return _cachedTransform;
			}
		}

		private void Update()
		{
			_positionUpToDate = false;
		}

		private void OnEnable()
		{
			Instances.Add(this);
		}

		private void OnDisable()
		{
			OnRemoved?.Invoke(this);
			Instances.Remove(this);
		}

		public override int GetHashCode()
		{
			return base.gameObject.GetHashCode();
		}
	}
}
