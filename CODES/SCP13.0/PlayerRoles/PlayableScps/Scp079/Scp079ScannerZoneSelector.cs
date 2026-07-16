using MapGeneration;
using Mirror;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ScannerZoneSelector : Scp079AbilityBase, IScp079AuxRegenModifier
	{
		private static readonly FacilityZone[] AllZones;

		private readonly bool[] _selectedZones;

		private string _regenPauseFormat;

		public string AuxReductionMessage => null;

		public float AuxRegenMultiplier => 0f;

		public int SelectedZonesCnt => 0;

		public FacilityZone[] SelectedZones => null;

		private int GetZoneIndex(FacilityZone zone)
		{
			return 0;
		}

		protected override void Awake()
		{
		}

		public bool GetZoneStatus(FacilityZone zone)
		{
			return false;
		}

		public void ToggleZoneStatus(FacilityZone zone)
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}
	}
}
