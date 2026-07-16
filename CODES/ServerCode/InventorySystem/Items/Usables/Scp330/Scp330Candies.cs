namespace InventorySystem.Items.Usables.Scp330
{
	public static class Scp330Candies
	{
		public static readonly global::InventorySystem.Items.Usables.Scp330.ICandy[] AllCandies = new global::InventorySystem.Items.Usables.Scp330.ICandy[7]
		{
			new global::InventorySystem.Items.Usables.Scp330.CandyGreen(),
			new global::InventorySystem.Items.Usables.Scp330.CandyPurple(),
			new global::InventorySystem.Items.Usables.Scp330.CandyRainbow(),
			new global::InventorySystem.Items.Usables.Scp330.CandyRed(),
			new global::InventorySystem.Items.Usables.Scp330.CandyYellow(),
			new global::InventorySystem.Items.Usables.Scp330.CandyBlue(),
			new global::InventorySystem.Items.Usables.Scp330.CandyPink()
		};

		private static bool _dictionarySet = false;

		private static readonly global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Usables.Scp330.CandyKindID, global::InventorySystem.Items.Usables.Scp330.ICandy> DictionarizedCandies = new global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Usables.Scp330.CandyKindID, global::InventorySystem.Items.Usables.Scp330.ICandy>();

		public static global::System.Collections.Generic.Dictionary<global::InventorySystem.Items.Usables.Scp330.CandyKindID, global::InventorySystem.Items.Usables.Scp330.ICandy> CandiesById
		{
			get
			{
				if (!_dictionarySet)
				{
					global::InventorySystem.Items.Usables.Scp330.ICandy[] allCandies = AllCandies;
					foreach (global::InventorySystem.Items.Usables.Scp330.ICandy candy in allCandies)
					{
						DictionarizedCandies[candy.Kind] = candy;
					}
					_dictionarySet = true;
				}
				return DictionarizedCandies;
			}
		}

		public static global::InventorySystem.Items.Usables.Scp330.CandyKindID GetRandom()
		{
			float num = 0f;
			global::InventorySystem.Items.Usables.Scp330.ICandy[] allCandies = AllCandies;
			foreach (global::InventorySystem.Items.Usables.Scp330.ICandy candy in allCandies)
			{
				num += candy.SpawnChanceWeight;
			}
			float num2 = global::UnityEngine.Random.Range(0f, num);
			allCandies = AllCandies;
			foreach (global::InventorySystem.Items.Usables.Scp330.ICandy candy2 in allCandies)
			{
				num2 -= candy2.SpawnChanceWeight;
				if (!(num2 > 0f))
				{
					return candy2.Kind;
				}
			}
			return global::InventorySystem.Items.Usables.Scp330.CandyKindID.None;
		}
	}
}
