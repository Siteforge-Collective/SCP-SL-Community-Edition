using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InventorySystem.Items.Firearms.Attachments
{
	public static class AttachmentsServerHandler
	{
		public static readonly Dictionary<ReferenceHub, Dictionary<ItemType, uint>> PlayerPreferences;

		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
		}

		private static void RegisterNetworkHandlers()
		{
		}

		private static void ServerReceiveChangeRequest(NetworkConnection conn, AttachmentsChangeRequest msg)
		{
		}

		private static void ServerReceivePreference(NetworkConnection conn, AttachmentsSetupPreference msg)
		{
		}

		private static void SetupProvidedWeapon(ReferenceHub ply, ItemBase item)
		{
		}

		private static void ResetOnSceneChange(Scene arg1, Scene arg2)
		{
		}
	}
}
