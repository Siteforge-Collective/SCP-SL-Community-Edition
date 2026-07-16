using System.Collections.Generic;
using GameObjectPools;
using UnityEngine;

public class PlayerModelCollisionController : MonoBehaviour, IPoolSpawnable, IPoolResettable
{
	public Collider[] MyColliders;

	public ReferenceHub Owner;

	public static PlayerModelCollisionController LocalPlayerController;

	public static List<PlayerModelCollisionController> AllControllers;

	public void SpawnObject()
	{
	}

	public void ResetObject()
	{
	}

	private void RefreshAllRelations()
	{
	}

	private void RefreshRelation(PlayerModelCollisionController targetController)
	{
	}
}
