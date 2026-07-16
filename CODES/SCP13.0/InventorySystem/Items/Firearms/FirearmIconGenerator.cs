using System;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Items.Firearms
{
	public static class FirearmIconGenerator
	{
		public static Vector2 GenerateIcon(this Firearm firearm, RawImage rootImage, RawImage[] imagePool, Vector2 maxSize, Func<int, Color> colorFunction)
		{
			return default(Vector2);
		}

		public static Bounds GenerateIcon(this Firearm firearm, RawImage rootImage, RawImage[] imagePool, Func<int, Color> colorFunction)
		{
			return default(Bounds);
		}

		private static void EncapsulateRect(ref Bounds b, RectTransform rct)
		{
		}
	}
}
