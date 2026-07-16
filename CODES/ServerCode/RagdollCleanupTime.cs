public class RagdollCleanupTime : global::UnityEngine.MonoBehaviour
{
	private string DisabledString;

	private string SecondsString;

	private string SecondString;

	public global::UnityEngine.UI.InputField Text;

	public global::UnityEngine.UI.Text SecondsText;

	public global::UnityEngine.UI.Slider Slider;

	public void Start()
	{
		DisabledString = TranslationReader.Get("NewMainMenu", 73, "Disabled");
		SecondsString = TranslationReader.Get("NewMainMenu", 74, "Seconds");
		SecondString = TranslationReader.Get("NewMainMenu", 86, "Second");
		int num = PlayerPrefsSl.Get("ragdoll_cleanup", 0);
		if (num <= 0)
		{
			Text.SetTextWithoutNotify(string.Empty);
			SecondsText.text = DisabledString;
		}
		else
		{
			SecondsText.text = ((num == 1) ? SecondString : SecondsString);
			Text.SetTextWithoutNotify(num.ToString());
			global::MEC.Timing.RunCoroutine(SetPositionOneFrameLater());
		}
		Slider.SetValueWithoutNotify(num);
	}

	public void OnValueChanged(float value)
	{
		global::UnityEngine.Mathf.Clamp(value, 0f, 604800f);
		if (value <= 0f)
		{
			Text.SetTextWithoutNotify(string.Empty);
			SecondsText.text = DisabledString;
		}
		else
		{
			SecondsText.text = ((value == 1f) ? SecondString : SecondsString);
			Text.SetTextWithoutNotify(value.ToString());
		}
		global::PlayerRoles.Ragdolls.RagdollManager.CleanupTime = (int)value;
		global::MEC.Timing.RunCoroutine(SetPositionOneFrameLater());
		Slider.SetValueWithoutNotify(value);
		PlayerPrefsSl.Set("ragdoll_cleanup", (int)value);
	}

	private global::System.Collections.Generic.IEnumerator<float> SetPositionOneFrameLater()
	{
		yield return float.NegativeInfinity;
		SecondsText.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(Text.textComponent.preferredWidth + 10f, 0f);
	}

	public void OnStringValueChanged(string value)
	{
		if (value.Length == 0)
		{
			OnValueChanged(0f);
		}
		if (float.TryParse(value, out var result))
		{
			OnValueChanged(result);
		}
	}
}
