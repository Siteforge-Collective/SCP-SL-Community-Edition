[global::UnityEngine.RequireComponent(typeof(global::TMPro.TMP_Dropdown))]
[global::UnityEngine.DisallowMultipleComponent]
public class DropDownController : global::UnityEngine.MonoBehaviour, global::UnityEngine.EventSystems.IPointerClickHandler, global::UnityEngine.EventSystems.IEventSystemHandler
{
	[global::UnityEngine.Tooltip("Indexes that should be ignored. Indexes are 0 based.")]
	public global::System.Collections.Generic.List<int> indexesToDisable = new global::System.Collections.Generic.List<int>();

	private global::TMPro.TMP_Dropdown _dropdown;

	private void Awake()
	{
		_dropdown = GetComponent<global::TMPro.TMP_Dropdown>();
	}

	public void OnPointerClick(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		global::UnityEngine.Canvas componentInChildren = GetComponentInChildren<global::UnityEngine.Canvas>();
		if ((bool)componentInChildren)
		{
			global::UnityEngine.UI.Toggle[] componentsInChildren = componentInChildren.GetComponentsInChildren<global::UnityEngine.UI.Toggle>(includeInactive: true);
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].interactable = !indexesToDisable.Contains(i - 1);
			}
		}
	}

	public void EnableOption(int index, bool enable)
	{
		if (index < 1 || index >= _dropdown.options.Count)
		{
			global::UnityEngine.Debug.LogWarning("Index out of range -> ignored!", this);
			return;
		}
		if (enable)
		{
			if (indexesToDisable.Contains(index))
			{
				indexesToDisable.Remove(index);
			}
		}
		else if (!indexesToDisable.Contains(index))
		{
			indexesToDisable.Add(index);
		}
		global::UnityEngine.Canvas componentInChildren = GetComponentInChildren<global::UnityEngine.Canvas>();
		if ((bool)componentInChildren)
		{
			componentInChildren.GetComponentsInChildren<global::UnityEngine.UI.Toggle>(includeInactive: true)[index].interactable = enable;
		}
	}

	public void EnableOption(string label, bool enable)
	{
		int num = _dropdown.options.FindIndex((global::TMPro.TMP_Dropdown.OptionData o) => string.Equals(o.text, label));
		EnableOption(num + 1, enable);
	}
}
