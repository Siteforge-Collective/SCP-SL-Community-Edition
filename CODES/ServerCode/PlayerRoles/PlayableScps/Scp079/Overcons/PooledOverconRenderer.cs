namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public abstract class PooledOverconRenderer : global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconRendererBase
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase _template;

		private readonly global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase> _queue = new global::System.Collections.Generic.Queue<global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase>();

		private readonly global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase> _spawned = new global::System.Collections.Generic.HashSet<global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase>();

		protected T GetFromPool<T>() where T : global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase
		{
			if (!CollectionExtensions.TryDequeue(_queue, out var element))
			{
				element = global::UnityEngine.Object.Instantiate(_template);
			}
			element.gameObject.SetActive(value: true);
			_spawned.Add(element);
			return element as T;
		}

		protected void ReturnToPool(global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase instance)
		{
			_spawned.Remove(instance);
			_queue.Enqueue(instance);
			instance.gameObject.SetActive(value: false);
		}

		protected void ReturnAll()
		{
			foreach (global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconBase item in _spawned)
			{
				if (!(item == null))
				{
					item.gameObject.SetActive(value: false);
					_queue.Enqueue(item);
				}
			}
			_spawned.Clear();
		}
	}
}
