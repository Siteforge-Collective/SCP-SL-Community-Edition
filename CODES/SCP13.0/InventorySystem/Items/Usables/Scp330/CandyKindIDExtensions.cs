using System.Linq;
using UnityEngine;

namespace InventorySystem.Items.Usables.Scp330
{
    public static class CandyKindIDExtensions
    {
        private static readonly CandyKindID[] AllCandies = {
        CandyKindID.Rainbow, CandyKindID.Yellow, CandyKindID.Purple,
        CandyKindID.Red, CandyKindID.Green, CandyKindID.Blue, CandyKindID.Pink };

        public static CandyKindID RandomExcluding(CandyKindID exclude)
        {
            var filtered = AllCandies.Where(c => c != exclude).ToArray();
            return filtered[Random.Range(0, filtered.Length)];
        }
    }
}
