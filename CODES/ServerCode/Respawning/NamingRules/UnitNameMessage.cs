namespace Respawning.NamingRules
{
	public struct UnitNameMessage : global::Mirror.NetworkMessage
	{
		public string UnitName;

		public global::Respawning.SpawnableTeamType Team;

		public global::Respawning.NamingRules.UnitNamingRule NamingRule;

		public global::Mirror.NetworkReader Data;
	}
}
