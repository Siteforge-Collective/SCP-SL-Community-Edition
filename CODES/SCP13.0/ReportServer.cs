using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ReportServer : MonoBehaviour
{
	private enum CentralResponse : byte
	{
		None = 0,
		Success = 1,
		Error = 2
	}

	public GameObject root;

	public TextMeshProUGUI ipAddress;

	public TextMeshProUGUI warningText;

	public GameObject description;

	public GameObject form;

	public GameObject report;

	public GameObject confirmation;

	private static int _reportedServersAmount;

	private static readonly Stopwatch ReportWatch;

	private static readonly HashSet<string> ReportedServers;

	private static readonly Regex Re;

	private static bool _waitingForResponse;

	private static float _responseTimer;

	private static CentralResponse _response;

	private static string _responseMessage;

	private void Update()
	{
	}

	public void Show(string ip)
	{
	}

	public void Close()
	{
	}

	private void ToggleForm(bool value)
	{
	}

	public void Proceed()
	{
	}

	public void Submit()
	{
	}

	private void ShowWarning(string message)
	{
	}

	private void ValidateReport(string serverIp, string violations, string explanation)
	{
	}

	private static void SubmitReport(ref string serverIp, ref string violations, ref string explanation)
	{
	}
}
