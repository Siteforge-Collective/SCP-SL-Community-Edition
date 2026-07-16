namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079NicknameGui : global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079GuiElementBase
	{
		private const float DefaultDistance = 17f;

		private const float AdditionalSize = 3f;

		private const float HeadSize = 270f;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Camera _cam;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _fullscreenRect;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.RectTransform _template;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync _camSync;

		private readonly global::System.Collections.Generic.Queue<global::UnityEngine.RectTransform> _pool = new global::System.Collections.Generic.Queue<global::UnityEngine.RectTransform>();

		private readonly global::System.Collections.Generic.HashSet<global::UnityEngine.RectTransform> _instances = new global::System.Collections.Generic.HashSet<global::UnityEngine.RectTransform>();

		private void LateUpdate()
		{
			Redraw();
		}

		private void Redraw()
		{
			ClearAll();
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.IsHuman() && allHub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole && fpcRole.FpcModule.CharacterModelInstance is global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel model)
				{
					TryDrawPlayer(model, allHub.nicknameSync.DisplayName);
				}
			}
		}

		private void TryDrawPlayer(global::PlayerRoles.FirstPersonControl.Thirdperson.HumanCharacterModel model, string nickname)
		{
			global::UnityEngine.Transform currentCamera = MainCameraController.CurrentCamera;
			float currentZoom = _camSync.CurrentCamera.ZoomAxis.CurrentZoom;
			float num = 17f * currentZoom;
			float num2 = num * num;
			HitboxIdentity[] hitboxes = model.Hitboxes;
			foreach (HitboxIdentity hitboxIdentity in hitboxes)
			{
				if (hitboxIdentity.HitboxType != HitboxType.Headshot)
				{
					continue;
				}
				if (hitboxIdentity.TargetColliders.Length != 0 && hitboxIdentity.TargetColliders[0] is global::UnityEngine.CapsuleCollider capsuleCollider)
				{
					global::UnityEngine.Vector3 vector = hitboxIdentity.transform.TransformPoint(capsuleCollider.center);
					if (!((vector - currentCamera.position).sqrMagnitude > num2) && !global::UnityEngine.Physics.Linecast(vector, currentCamera.position, global::PlayerRoles.PlayableScps.VisionInformation.VisionLayerMask))
					{
						DrawRectangle(vector, currentZoom, nickname);
					}
				}
				break;
			}
		}

		private void DrawRectangle(global::UnityEngine.Vector3 targetPos, float zoom, string nickname)
		{
			global::UnityEngine.Vector2 sizeDelta = _fullscreenRect.sizeDelta;
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(sizeDelta.x / (float)global::UnityEngine.Screen.width, sizeDelta.y / (float)global::UnityEngine.Screen.height);
			global::UnityEngine.Vector3 vector2 = targetPos - _camSync.CurrentCamera.Position;
			float magnitude = vector2.magnitude;
			if (magnitude != 0f && !(global::UnityEngine.Vector3.Dot(MainCameraController.CurrentCamera.forward, vector2 / magnitude) < 0f))
			{
				if (!CollectionExtensions.TryDequeue(_pool, out var element))
				{
					element = global::UnityEngine.Object.Instantiate(_template, _template.parent);
				}
				global::UnityEngine.Vector3 vector3 = _cam.WorldToScreenPoint(targetPos);
				float num = 270f * zoom / magnitude + 3f;
				element.anchoredPosition = vector3 * vector;
				element.sizeDelta = num * global::UnityEngine.Vector2.one;
				element.gameObject.SetActive(value: true);
				bool flag = new global::UnityEngine.Bounds(vector3, global::UnityEngine.Vector3.one * num).Contains(global::UnityEngine.Input.mousePosition);
				element.GetComponentInChildren<global::TMPro.TextMeshProUGUI>().text = (flag ? nickname : string.Empty);
				_instances.Add(element);
			}
		}

		private void ClearAll()
		{
			foreach (global::UnityEngine.RectTransform instance in _instances)
			{
				instance.gameObject.SetActive(value: false);
				_pool.Enqueue(instance);
			}
			_instances.Clear();
		}

		internal override void Init(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, ReferenceHub owner)
		{
			base.Init(role, owner);
			role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync>(out _camSync);
		}
	}
}
