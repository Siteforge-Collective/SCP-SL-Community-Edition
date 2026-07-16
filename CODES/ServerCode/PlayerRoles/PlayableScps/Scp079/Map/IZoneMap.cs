namespace PlayerRoles.PlayableScps.Scp079.Map
{
	public interface IZoneMap
	{
		void Generate();

		bool TryGetCenterTransform(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam, out global::UnityEngine.Vector3 center);

		bool TrySetTeammateIndicator(ReferenceHub ply, global::UnityEngine.RectTransform indicator, bool exact);

		void UpdateOpened(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera curCam);

		bool TryGetCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera target);
	}
}
