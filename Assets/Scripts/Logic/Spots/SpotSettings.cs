using Data.DataLoot;
using UnityEngine;

namespace Logic.Spots
{
    [CreateAssetMenu(fileName = "SpotSettings", menuName = "Spot/SpotSettings", order = 0)]
    public class SpotSettings : ScriptableObject
    {
        [field: SerializeField] public Loot TotalRequiredLoot { get; private set; }
        [field: SerializeField] public Loot RefinedLoot { get; private set; }
        [field: SerializeField] public float RefineTime { get; private set; }
    }
}