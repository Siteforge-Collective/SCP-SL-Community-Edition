using System.Collections.Generic;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Overcons
{
	public abstract class PooledOverconRenderer : OverconRendererBase
	{
		[SerializeField]
		private OverconBase _template;

		private readonly Queue<OverconBase> _queue;

		private readonly HashSet<OverconBase> _spawned;

		protected T GetFromPool<T>() where T : OverconBase
		{
			return null;
		}

		protected void ReturnToPool(OverconBase instance)
		{
		}

		protected void ReturnAll()
		{
		}
	}
}
