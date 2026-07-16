using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.Subroutines;
using UnityEngine;

public class Scp106Breathing : StandardSubroutine<Scp106Role>
{
	private const float SelfMaxVolume = 0.5f;

	private const float MaxVolume = 1f;

	private Scp106SinkholeController _sinkholeController;

	[SerializeField]
	private AudioSource _breathingSource;

	protected override void Awake()
	{
	}

	private void Update()
	{
	}
}
