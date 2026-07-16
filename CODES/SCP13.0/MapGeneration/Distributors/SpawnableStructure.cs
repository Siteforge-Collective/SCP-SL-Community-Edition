using Mirror;
using UnityEngine;

namespace MapGeneration.Distributors
{
    [RequireComponent(typeof(StructurePositionSync))]
    public class SpawnableStructure : NetworkBehaviour
    {
        public StructureType StructureType;

        [Tooltip("Defines the number of minimum and maximum amounts of instances of this structure. The generator chooses a random horizontal point between 0 to 1, and reads its vertical value.")]
        public AnimationCurve MinMaxProbability = AnimationCurve.Constant(0f, 0f, 0f);

        public int MinAmount => Mathf.FloorToInt(MinMaxProbability.Evaluate(0f));

        public int MaxAmount => Mathf.FloorToInt(MinMaxProbability.Evaluate(1f));
    }
}
