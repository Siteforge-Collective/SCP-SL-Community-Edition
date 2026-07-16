using System.Collections.Generic;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079Speaker : Scp079InteractableBase
	{
		private static readonly Dictionary<RoomIdentifier, List<Scp079Speaker>> SpeakersInRooms;

		private bool _wasRegistered;

		protected override void OnRegistered()
		{
		}

		protected override void OnDestroy()
		{
		}

		public static bool TryGetSpeaker(Scp079Camera cam, out Scp079Speaker best)
		{
			best = null;
			return false;
		}
	}
}
