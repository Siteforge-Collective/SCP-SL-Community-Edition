using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Jailbird
{
	[Serializable]
	public class JailbirdDeteriorationTracker
	{
		public static Dictionary<ushort, JailbirdWearState> ReceivedStates;

		private static bool _anyReceived;

		private static bool _914ValuesSet;

		private JailbirdItem _jailbird;

		private JailbirdHitreg _hitreg;

		[SerializeField]
		private AnimationCurve _damageToWearState;

		[SerializeField]
		private AnimationCurve _chargesToWearState;

		public static float Scp914CoarseDamage { get; private set; }

		public static int Scp914CoarseCharges { get; private set; }

		public JailbirdWearState WearState => default(JailbirdWearState);

		public bool IsBroken => false;

		public void Setup(JailbirdItem item, JailbirdHitreg hitreg)
		{
		}

		private JailbirdWearState FloatToState(float stateFloat)
		{
			return default(JailbirdWearState);
		}

		private JailbirdWearState StateForTotalDamage(float totalDamage)
		{
			return default(JailbirdWearState);
		}

		private JailbirdWearState StateForCharges(int numOfCharges)
		{
			return default(JailbirdWearState);
		}

		public void RecheckUsage()
		{
		}

		public static void ReadUsage(ushort serial, NetworkReader reader)
		{
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}
	}
}
