using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RemoteAdmin
{
	public class PlayerRequest : MonoBehaviour
	{
		public Transform parent;

		public GameObject template;

		public static PlayerRequest Singleton;

		public TMP_InputField playerSearch;

		private readonly List<GameObject> _spawnedObjects;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		public void ResponsePlayerList(string data, bool isSuccess, bool showClasses)
		{
		}
	}
}
