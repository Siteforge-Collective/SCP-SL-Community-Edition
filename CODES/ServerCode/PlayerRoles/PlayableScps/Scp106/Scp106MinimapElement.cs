namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106MinimapElement : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IPointerEnterHandler, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IPointerExitHandler
	{
		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.UI.Image Img { get; private set; }

		[field: global::UnityEngine.SerializeField]
		public global::UnityEngine.RectTransform Rt { get; private set; }

		public global::MapGeneration.RoomIdentifier Room { get; internal set; }

		public static bool AnyHighlighted { get; private set; }

		public static global::PlayerRoles.PlayableScps.Scp106.Scp106MinimapElement LastHighlighted { get; private set; }

		public void OnPointerEnter(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			LastHighlighted = this;
			AnyHighlighted = true;
		}

		public void OnPointerExit(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (!(LastHighlighted != this))
			{
				LastHighlighted = null;
				AnyHighlighted = false;
			}
		}
	}
}
