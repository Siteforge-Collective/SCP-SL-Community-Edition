namespace GameCore
{
	public static class Version
	{
		public enum VersionType : byte
		{
			Release = 0,
			PublicRC = 1,
			PublicBeta = 2,
			PrivateRC = 3,
			PrivateRCStreamingForbidden = 4,
			PrivateBeta = 5,
			PrivateBetaStreamingForbidden = 6,
			Development = 7,
			Nightly = 8
		}

		public static readonly byte Major;

		public static readonly byte Minor;

		public static readonly byte Revision;

		public static readonly bool AlwaysAcceptReleaseBuilds;

		public static readonly global::GameCore.Version.VersionType BuildType;

		public static readonly bool BackwardCompatibility;

		public static readonly byte BackwardRevision;

		public static readonly string DescriptionOverride;

		public static readonly string VersionString;

		public static bool PublicBeta
		{
			get
			{
				if (BuildType != global::GameCore.Version.VersionType.PublicBeta)
				{
					return BuildType == global::GameCore.Version.VersionType.PublicRC;
				}
				return true;
			}
		}

		public static bool PrivateBeta
		{
			get
			{
				if (BuildType != global::GameCore.Version.VersionType.PrivateBeta && BuildType != global::GameCore.Version.VersionType.PrivateBetaStreamingForbidden && BuildType != global::GameCore.Version.VersionType.PrivateRC && BuildType != global::GameCore.Version.VersionType.PrivateRCStreamingForbidden && BuildType != global::GameCore.Version.VersionType.Development)
				{
					return BuildType == global::GameCore.Version.VersionType.Nightly;
				}
				return true;
			}
		}

		public static bool ReleaseCandidate
		{
			get
			{
				if (BuildType != global::GameCore.Version.VersionType.PublicRC && BuildType != global::GameCore.Version.VersionType.PrivateRC)
				{
					return BuildType == global::GameCore.Version.VersionType.PrivateRCStreamingForbidden;
				}
				return true;
			}
		}

		public static bool StreamingAllowed
		{
			get
			{
				if (BuildType != global::GameCore.Version.VersionType.PrivateBetaStreamingForbidden)
				{
					return BuildType != global::GameCore.Version.VersionType.PrivateRCStreamingForbidden;
				}
				return false;
			}
		}

		public static bool ExtendedVersionCheckNeeded => BuildType != global::GameCore.Version.VersionType.Release;

		static Version()
		{
			Major = 12;
			Minor = 0;
			Revision = 2;
			AlwaysAcceptReleaseBuilds = true;
			BuildType = global::GameCore.Version.VersionType.Release;
			BackwardCompatibility = false;
			BackwardRevision = 0;
			DescriptionOverride = null;
			VersionString = string.Format("{0}.{1}.{2}{3}", Major, Minor, Revision, (!ExtendedVersionCheckNeeded) ? string.Empty : ("-" + (DescriptionOverride ?? "version-check-e6b2ce61")));
		}

		public static bool CompatibilityCheck(byte sMajor, byte sMinor, byte sRevision, byte cMajor, byte cMinor, byte cRevision, bool cBackwardEnabled, byte cBackwardRevision)
		{
			if (sMajor != cMajor || sMinor != cMinor)
			{
				return false;
			}
			if (!cBackwardEnabled)
			{
				return sRevision == cRevision;
			}
			if (sRevision >= cBackwardRevision)
			{
				return sRevision <= cRevision;
			}
			return false;
		}
	}
}
