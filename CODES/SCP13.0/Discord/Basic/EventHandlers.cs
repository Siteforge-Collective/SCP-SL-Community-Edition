using System;

namespace Discord.Basic
{
	public struct EventHandlers : IEquatable<EventHandlers>
	{
		public CallbackController.OnReadyInfo readyCallback;

		public CallbackController.OnDisconnectedInfo disconnectedCallback;

		public CallbackController.OnErrorInfo errorCallback;

		public CallbackController.OnJoinInfo joinCallback;

		public CallbackController.OnSpectateInfo spectateCallback;

		public CallbackController.OnRequestInfo requestCallback;

		public bool Equals(EventHandlers other)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(EventHandlers left, EventHandlers right)
		{
			return false;
		}

		public static bool operator !=(EventHandlers left, EventHandlers right)
		{
			return false;
		}
	}
}
