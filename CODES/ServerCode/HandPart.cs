public class HandPart : global::InventorySystem.Items.Thirdperson.ThirdpersonItemBase
{
	public global::UnityEngine.GameObject TargetPart;

	public ItemType TargetItemId;

	public bool UseUniversalAnimations;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject optionalPrefab;

	protected bool CurrentlyEnabled { get; set; }

	protected global::UnityEngine.GameObject SpawnedObject { get; set; }

	public void UpdateItem()
	{
		bool flag = base.TargetModel.OwnerHub.inventory.CurItem.TypeId == TargetItemId;
		if (flag == CurrentlyEnabled)
		{
			return;
		}
		CurrentlyEnabled = flag;
		TargetPart.SetActive(flag);
		if (flag && optionalPrefab != null)
		{
			SpawnedObject = (global::GameObjectPools.PoolManager.Singleton.TryGetPoolObject(optionalPrefab, TargetPart.transform, out var poolObject) ? poolObject.gameObject : global::UnityEngine.Object.Instantiate(optionalPrefab, TargetPart.transform));
			SpawnedObject.transform.localScale = global::UnityEngine.Vector3.one;
			SpawnedObject.transform.localPosition = global::UnityEngine.Vector3.zero;
			SpawnedObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
			OnActiveStateChange(isEnabled: true);
		}
		else if (SpawnedObject != null)
		{
			OnActiveStateChange(isEnabled: false);
			if (!global::GameObjectPools.PoolManager.Singleton.TryReturnPoolObject(SpawnedObject))
			{
				global::UnityEngine.Object.Destroy(SpawnedObject);
			}
		}
	}

	public override void ResetObject()
	{
		base.ResetObject();
		TargetPart.SetActive(value: false);
		CurrentlyEnabled = false;
		OnActiveStateChange(isEnabled: false);
	}

	protected virtual void OnActiveStateChange(bool isEnabled)
	{
	}

	public override float GetTransitionTime(global::InventorySystem.Items.ItemIdentifier iid)
	{
		return 2f;
	}
}
