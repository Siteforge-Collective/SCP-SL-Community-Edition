using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MapGeneration.Distributors;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079GeneratorNotification : Scp079SimpleNotification
	{
		public static readonly HashSet<Scp079Generator> TrackedGens;

		private readonly Scp079Generator _generator;

		private readonly StringBuilder _emptyBuilder;

		private readonly Stopwatch _activeStopwatch;

		private const string UnknownRoom = "UNKNOWN";

		private const string Format = "<color=red>0m 00s - {0}</color>";

		private const int MinuteDigit = 11;

		private const int TensDigit = 14;

		private const int SecsDigit = 15;

		private const int CharOffset = 48;

		private const int BlinkRate = 9;

		private const float BlinkDuration = 2.5f;

		private float _opacity;

		private bool IsActivating => false;

		public override float Opacity => 0f;

		public override bool Delete => false;

		protected override StringBuilder WrittenText => null;

		public Scp079GeneratorNotification(Scp079Generator generator, bool skipAnimation)
			: base(null)
		{
		}

		private static void OverrideStringBuilder(StringBuilder sb, int place, int character)
		{
		}

		private static string GetGeneratorCamera(Scp079Generator gen)
		{
			return null;
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
