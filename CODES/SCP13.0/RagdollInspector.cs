using PlayerRoles;
using PlayerRoles.Ragdolls;
using TMPro;
using UnityEngine;

public class RagdollInspector : MonoBehaviour
{
	[SerializeField]
	private LayerMask _raycastMask;

	[SerializeField]
	private float _raycastDistance;

	[SerializeField]
	private TextMeshProUGUI _ragdollInspectText;

	private bool _isVisible;

	public static string DefaultFormat => null;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnRaycastHit(RaycastHit hit)
	{
	}

	private void Show(BasicRagdoll ragdoll, PlayerRoleBase role)
	{
	}

	private void Hide()
	{
	}

	private bool TryGetRagdoll(RaycastHit hit, out BasicRagdoll ragdoll, out PlayerRoleBase role)
	{
		ragdoll = null;
		role = null;
		return false;
	}

	private string GetFormatOverride<T>(T obj) where T : class
	{
		return null;
	}
}
