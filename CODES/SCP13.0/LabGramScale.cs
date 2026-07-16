using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class LabGramScale : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private int _weightOffset;

	[SerializeField]
	private AudioSource _beep;

	private float _detectedWeight;

	private float _prevDetected;

	private float _weightRandom;

	private bool _idle;

	private const float AdjustmentLerp = 1.4f;

	private const int WeightLimitGrams = 4500;

	private const int SaladLimit = 14000;

	private const float DisableTime = 5f;

	private const float ConversionRate = 945f;

	private readonly Stopwatch _lastWeightSw;

	private readonly HashSet<Rigidbody> _detectedRbs;

	private bool Calibrated => false;

	private void OnTriggerStay(Collider other)
	{
	}

	private void FixedUpdate()
	{
	}
}
