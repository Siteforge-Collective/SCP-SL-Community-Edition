public class StatusBar : global::UnityEngine.MonoBehaviour
{
	public enum AutoHideType
	{
		WhenFull = 0,
		WhenEmpty = 1,
		AlwaysVisible = 2
	}

	[global::UnityEngine.Tooltip("Above which object should this be displayed?")]
	public StatusBar MasterBar;

	[global::UnityEngine.Tooltip("Slider which the script will affect")]
	public global::UnityEngine.UI.Slider TargetSlider;

	[global::UnityEngine.Tooltip("All graphics that should be faded in/out")]
	public global::UnityEngine.CanvasRenderer[] TargetGraphics;

	[global::UnityEngine.Tooltip("Y-axis offset of the MasterTransform.position")]
	public float FixedDistance;

	public StatusBar.AutoHideType AutohideOption;

	public bool HiddenByDefault;

	public float FadeInSpeed;

	public float FadeOutSpeed;

	public float MoveSpeed;

	private global::UnityEngine.RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<global::UnityEngine.RectTransform>();
	}

	private void OnEnable()
	{
		if (HiddenByDefault)
		{
			global::UnityEngine.CanvasRenderer[] targetGraphics = TargetGraphics;
			for (int i = 0; i < targetGraphics.Length; i++)
			{
				targetGraphics[i].SetAlpha(0f);
			}
		}
	}

	private void Move(float targetY, bool bypassAnimation)
	{
		global::UnityEngine.Vector3 localPosition = rectTransform.localPosition;
		if (bypassAnimation)
		{
			localPosition.y = targetY;
		}
		else
		{
			float num = global::UnityEngine.Time.deltaTime * MoveSpeed;
			float num2 = targetY - localPosition.y;
			if (num > global::UnityEngine.Mathf.Abs(num2))
			{
				localPosition.y = targetY;
			}
			else
			{
				localPosition.y += num * (float)((num2 > 0f) ? 1 : (-1));
			}
		}
		rectTransform.localPosition = localPosition;
	}

	private void Update()
	{
		UpdateBar(bypassAnims: false);
	}

	public void UpdateBar(bool bypassAnims)
	{
		if (MasterBar != null)
		{
			StatusBar masterBar = MasterBar;
			while (masterBar.TargetGraphics.Length != 0 && masterBar.TargetGraphics[0].GetAlpha() <= 0.1f)
			{
				if (masterBar.MasterBar == null)
				{
					return;
				}
				masterBar = masterBar.MasterBar;
			}
			Move(masterBar.rectTransform.localPosition.y + FixedDistance, bypassAnims || TargetGraphics[0].GetAlpha() <= 0f);
		}
		if (AutohideOption != StatusBar.AutoHideType.AlwaysVisible)
		{
			float num = FadeInSpeed;
			if ((AutohideOption == StatusBar.AutoHideType.WhenEmpty && TargetSlider.value == TargetSlider.minValue) || (AutohideOption == StatusBar.AutoHideType.WhenFull && TargetSlider.value == TargetSlider.maxValue))
			{
				num = 0f - FadeOutSpeed;
			}
			if (bypassAnims)
			{
				SetAlpha(global::UnityEngine.Mathf.Sign(num));
			}
			else
			{
				SetAlpha(TargetGraphics[0].GetAlpha() + num * global::UnityEngine.Time.deltaTime);
			}
		}
	}

	public void SetAlpha(float a)
	{
		global::UnityEngine.CanvasRenderer[] targetGraphics = TargetGraphics;
		for (int i = 0; i < targetGraphics.Length; i++)
		{
			targetGraphics[i].SetAlpha(global::UnityEngine.Mathf.Clamp01(a));
		}
	}
}
