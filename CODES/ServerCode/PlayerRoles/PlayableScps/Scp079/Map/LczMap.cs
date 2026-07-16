namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class LczMap : global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap
	{
		private const float UnnamedAlpha = 0.2f;

		private const float UnnamedSize = 0.7f;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap _hczMap;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp079.Map.ProceduralZoneMap _ezMap;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2 _spacing;

		protected override void PlaceRooms()
		{
			base.PlaceRooms();
		}

		protected override void PostProcessRooms()
		{
			base.PostProcessRooms();
			global::UnityEngine.Vector2 vector = _hczMap.RectBounds.center - base.RectBounds.center;
			vector += global::UnityEngine.Vector2.up * (_spacing.y + _hczMap.RectBounds.extents.y + base.RectBounds.extents.y);
			vector += global::UnityEngine.Vector2.right * (base.RectBounds.extents.x + _spacing.x - _hczMap.RectBounds.extents.x);
			foreach (global::System.Collections.Generic.KeyValuePair<global::MapGeneration.RoomIdentifier, global::UnityEngine.UI.Image> item in NodesByRoom)
			{
				ProcessName(item.Key, item.Value);
				item.Value.rectTransform.anchoredPosition += vector;
			}
			ZoneLabel.rectTransform.anchoredPosition += vector;
			base.RectBounds = new global::UnityEngine.Bounds(base.RectBounds.center + (global::UnityEngine.Vector3)vector, base.RectBounds.size);
		}

		private void ProcessName(global::MapGeneration.RoomIdentifier room, global::UnityEngine.UI.Image img)
		{
			if (room.Name == global::MapGeneration.RoomName.Unnamed)
			{
				string text = room.name;
				text = text.Remove(0, text.IndexOf('(') + 1);
				text = text.Remove(text.IndexOf(')'));
				if (int.TryParse(text, out var result))
				{
					global::TMPro.TextMeshProUGUI componentInChildren = img.GetComponentInChildren<global::TMPro.TextMeshProUGUI>();
					componentInChildren.text = string.Format(componentInChildren.text, result);
					componentInChildren.alpha = 0.2f;
					componentInChildren.fontSize *= 0.7f;
				}
			}
		}
	}
}
