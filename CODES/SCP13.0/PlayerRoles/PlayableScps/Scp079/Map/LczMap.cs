using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public class LczMap : ProceduralZoneMap
	{
		private const float UnnamedAlpha = 0.2f;

		private const float UnnamedSize = 0.7f;

		[SerializeField]
		private ProceduralZoneMap _hczMap;

		[SerializeField]
		private ProceduralZoneMap _ezMap;

		[SerializeField]
		private Vector2 _spacing;

		protected override void PlaceRooms()
		{
		}

		protected override void PostProcessRooms()
		{
		}

		private void ProcessName(RoomNode node)
		{
		}
	}
}
