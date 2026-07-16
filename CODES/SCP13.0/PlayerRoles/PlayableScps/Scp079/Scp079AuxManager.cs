using System.Text;
using Mirror;
using PlayerRoles.PlayableScps.Scp079.GUI;
using PlayerRoles.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079AuxManager : StandardSubroutine<Scp079Role>, IScp079LevelUpNotifier
	{
		[SerializeField]
		private float[] _regenerationPerTier;

		[SerializeField]
		private float[] _maxPerTier;

		private Scp079TierManager _tierManager;

		private IScp079AuxRegenModifier[] _abilities;

		private int _abilitiesCount;

		private float _aux;

		private bool _valueDirty;

		private ushort _prevSent;

		private static string _textEtaFormat;

		private static string _textHigherTierRequired;

		private static string _textNewMaxAux;

		private ushort Compressed => 0;

		private float RegenSpeed => 0f;

		public float CurrentAux
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public int CurrentAuxFloored => 0;

		public float MaxAux => 0f;

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void Regenerate()
		{
		}

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase prev, PlayerRoleBase cur)
		{
		}

		private void SyncValues()
		{
		}

		public string GenerateETA(float requiredAux)
		{
			return null;
		}

		public string GenerateCustomETA(int secondsRemaining)
		{
			return null;
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public bool WriteLevelUpNotification(StringBuilder sb, int newLevel)
		{
			return false;
		}
	}
}
