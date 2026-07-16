namespace PlayerRoles.Spectating
{
	public class SpectatableListManager : global::UnityEngine.MonoBehaviour
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.Spectating.SpectatableListElementDefinition[] _definedPairs;

		[global::UnityEngine.SerializeField]
		private float _targetHeight;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.VerticalLayoutGroup _layoutGroup;

		private int _lastTargetId;

		private readonly global::System.Collections.Generic.List<global::PlayerRoles.Spectating.SpectatableListSpawnedElement> _spawnedTargets = new global::System.Collections.Generic.List<global::PlayerRoles.Spectating.SpectatableListSpawnedElement>();

		private static bool _initialized;

		private static global::UnityEngine.KeyCode _nextKey;

		private static global::UnityEngine.KeyCode _prevKey;

		private static readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.Spectating.SpectatableListElementType, global::PlayerRoles.Spectating.SpectatableListElementDefinition> Definitions = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.Spectating.SpectatableListElementType, global::PlayerRoles.Spectating.SpectatableListElementDefinition>();

		private void OnEnable()
		{
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(RefreshSize));
			global::PlayerRoles.Spectating.SpectatableModuleBase.OnAdded += AddTarget;
			global::PlayerRoles.Spectating.SpectatableModuleBase.OnRemoved += RemoveTarget;
			if (!_initialized)
			{
				global::PlayerRoles.Spectating.SpectatableListElementDefinition[] definedPairs = _definedPairs;
				for (int i = 0; i < definedPairs.Length; i++)
				{
					global::PlayerRoles.Spectating.SpectatableListElementDefinition value = definedPairs[i];
					Definitions[value.Type] = value;
					global::GameObjectPools.PoolManager.Singleton.TryAddPool(value.FullSize);
					global::GameObjectPools.PoolManager.Singleton.TryAddPool(value.Compact);
				}
				RefreshKeybinds();
				_initialized = true;
			}
			RefreshAllTargets();
		}

		private void OnDisable()
		{
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Remove(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(RefreshSize));
			global::PlayerRoles.Spectating.SpectatableModuleBase.OnAdded -= AddTarget;
			global::PlayerRoles.Spectating.SpectatableModuleBase.OnRemoved -= RemoveTarget;
		}

		private void LateUpdate()
		{
		}

		private void AddTarget(global::PlayerRoles.Spectating.SpectatableModuleBase target)
		{
			int orderPriority = GetOrderPriority(target.MainRole);
			int num = _spawnedTargets.Count;
			for (int i = 0; i < _spawnedTargets.Count; i++)
			{
				if (_spawnedTargets[i].Priority > orderPriority)
				{
					num = i;
					break;
				}
			}
			if (Definitions.TryGetValue(target.ListElementType, out var value) && value.TryGetFromPools(base.transform, out var full, out var compact))
			{
				global::PlayerRoles.Spectating.SpectatableListSpawnedElement item = new global::PlayerRoles.Spectating.SpectatableListSpawnedElement
				{
					Priority = orderPriority,
					Compact = compact,
					FullSize = full,
					Target = target
				};
				SetupNewTarget(item.Compact, target, num * 2);
				SetupNewTarget(item.FullSize, target, num * 2 + 1);
				_spawnedTargets.Insert(num, item);
				RefreshSize();
			}
		}

		private void RemoveTarget(global::PlayerRoles.Spectating.SpectatableModuleBase target)
		{
			for (int i = 0; i < _spawnedTargets.Count; i++)
			{
				if (!(_spawnedTargets[i].Compact.Target != target))
				{
					_spawnedTargets[i].ReturnToPool();
					_spawnedTargets.RemoveAt(i);
					break;
				}
			}
			RefreshSize();
		}

		private void RefreshAllTargets()
		{
			_spawnedTargets.ForEach(delegate(global::PlayerRoles.Spectating.SpectatableListSpawnedElement x)
			{
				x.ReturnToPool();
			});
			_spawnedTargets.Clear();
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.roleManager.CurrentRole is global::PlayerRoles.Spectating.ISpectatableRole spectatableRole)
				{
					AddTarget(spectatableRole.SpectatorModule);
				}
			}
		}

		private void SetupNewTarget(global::PlayerRoles.Spectating.SpectatableListElementBase element, global::PlayerRoles.Spectating.SpectatableModuleBase module, int order)
		{
			global::UnityEngine.Transform obj = element.transform;
			obj.localScale = global::UnityEngine.Vector3.one;
			obj.localPosition = global::UnityEngine.Vector3.zero;
			obj.localRotation = global::UnityEngine.Quaternion.identity;
			obj.SetSiblingIndex(order);
			element.Index = order;
			element.Target = module;
		}

		private void RefreshSize()
		{
			int count = _spawnedTargets.Count;
			if (count == 0)
			{
				return;
			}
			float num2;
			float num = (num2 = _layoutGroup.spacing * (float)(count - 1));
			for (int i = 0; i < count; i++)
			{
				global::PlayerRoles.Spectating.SpectatableListSpawnedElement spectatableListSpawnedElement = _spawnedTargets[i];
				num2 += spectatableListSpawnedElement.Compact.Height;
				num += spectatableListSpawnedElement.FullSize.Height;
				if (_spawnedTargets[i].Target == global::PlayerRoles.Spectating.SpectatorTargetTracker.CurrentTarget)
				{
					_lastTargetId = i;
				}
			}
			float num3 = global::UnityEngine.Mathf.InverseLerp(num2, num, _targetHeight);
			int num6;
			int num7;
			if (num3 < 1f)
			{
				int num4 = global::UnityEngine.Mathf.FloorToInt(num3 * (float)count) - 1;
				if (num4 > 12)
				{
					num4 = 12;
				}
				else if (num4 % 2 != 0)
				{
					num4--;
				}
				int num5 = global::UnityEngine.Mathf.Clamp(_lastTargetId, 0, count - 1);
				num6 = num5 - num4 / 2;
				num7 = num5 + num4 / 2;
				if (num6 < 0)
				{
					num7 -= num6;
				}
				if (num7 >= count)
				{
					num6 += 1 + count - num7;
				}
			}
			else
			{
				num6 = 0;
				num7 = count;
			}
			for (int j = 0; j < count; j++)
			{
				bool flag = j >= num6 && j <= num7;
				_spawnedTargets[j].FullSize.gameObject.SetActive(flag);
				_spawnedTargets[j].Compact.gameObject.SetActive(!flag);
			}
		}

		private static void RefreshKeybinds()
		{
			_nextKey = NewInput.GetKey(ActionName.Shoot);
			_prevKey = NewInput.GetKey(ActionName.Zoom);
		}

		private static int GetOrderPriority(global::PlayerRoles.PlayerRoleBase prb)
		{
			int num = (int)prb.Team.GetFaction() * 65535;
			int num2 = (int)prb.Team * 255;
			int roleTypeId = (int)prb.RoleTypeId;
			return num + num2 + roleTypeId;
		}
	}
}
