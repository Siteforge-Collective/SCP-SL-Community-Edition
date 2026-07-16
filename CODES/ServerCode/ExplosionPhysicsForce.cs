public class ExplosionPhysicsForce : global::UnityEngine.MonoBehaviour
{
	public float explosionForce = 4f;

	private global::System.Collections.IEnumerator Start()
	{
		yield return null;
		float num = 0f;
		if (GetComponent<global::UnityStandardAssets.Effects.ParticleSystemMultiplier>() != null)
		{
			num = GetComponent<global::UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier;
		}
		float num2 = 10f * num;
		global::UnityEngine.Collider[] array = global::UnityEngine.Physics.OverlapSphere(base.transform.position, num2);
		global::System.Collections.Generic.List<global::UnityEngine.Rigidbody> list = global::NorthwoodLib.Pools.ListPool<global::UnityEngine.Rigidbody>.Shared.Rent();
		global::UnityEngine.Collider[] array2 = array;
		foreach (global::UnityEngine.Collider collider in array2)
		{
			if (collider.attachedRigidbody != null && !list.Contains(collider.attachedRigidbody))
			{
				list.Add(collider.attachedRigidbody);
			}
		}
		foreach (global::UnityEngine.Rigidbody item in list)
		{
			item.AddExplosionForce(explosionForce * num, base.transform.position, num2, 1f * num, global::UnityEngine.ForceMode.Impulse);
		}
		global::NorthwoodLib.Pools.ListPool<global::UnityEngine.Rigidbody>.Shared.Return(list);
	}
}
