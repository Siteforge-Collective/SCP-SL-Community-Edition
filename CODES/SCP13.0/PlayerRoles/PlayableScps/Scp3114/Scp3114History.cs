using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Footprinting;
using PlayerRoles.Subroutines;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114History : StandardSubroutine<Scp3114Role>
	{
		private record RoundInstance(Footprint OwnerFootprint, List<LoggedIdentity> History)
		{
			protected virtual Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return null;
				}
			}

			public Footprint OwnerFootprint { get; set; }

			public List<LoggedIdentity> History { get; set; }

			public string PrintInstanceHistory(int selfId)
			{
				return null;
			}

			public override string ToString()
			{
				return null;
			}

			protected virtual bool PrintMembers(StringBuilder builder)
			{
				return false;
			}

			public virtual bool Equals(RoundInstance? other)
			{
				return false;
			}

			protected RoundInstance(RoundInstance original)
			{
			}

			public void Deconstruct(out Footprint OwnerFootprint, out List<LoggedIdentity> History)
			{
				OwnerFootprint = default(Footprint);
				History = null;
			}
		}

		private record LoggedIdentity(string Nickname, RoleTypeId Role, Stopwatch Time)
		{
			protected virtual Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return null;
				}
			}

			public string Nickname { get; set; }

			public RoleTypeId Role { get; set; }

			public Stopwatch Time { get; set; }

			public void AppendSelf(StringBuilder sb)
			{
			}

			public override string ToString()
			{
				return null;
			}

			protected virtual bool PrintMembers(StringBuilder builder)
			{
				return false;
			}

			public virtual bool Equals(LoggedIdentity? other)
			{
				return false;
			}

			protected LoggedIdentity(LoggedIdentity original)
			{
			}

			public void Deconstruct(out string Nickname, out RoleTypeId Role, out Stopwatch Time)
			{
				Nickname = null;
				Role = default(RoleTypeId);
				Time = null;
			}
		}

		private static readonly List<RoundInstance> RoundOverallHistory;

		private static int _prevRoundId;

		private int _historyIndex;

		private List<LoggedIdentity> History => null;

		private void OnStatusChanged()
		{
		}

		private void ServerLogIdentity(string msg)
		{
		}

		private static void AppendTime(StringBuilder sb, TimeSpan elapsed)
		{
		}

		private static string PrintAllInstances()
		{
			return null;
		}

		public static string PrintHistory(int? specificInstance)
		{
			return null;
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
