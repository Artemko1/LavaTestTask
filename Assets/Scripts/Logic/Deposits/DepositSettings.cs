using Data.DataLoot;
using UnityEngine;

namespace Logic.Deposits
{
    [CreateAssetMenu(fileName = "DepositSettings", menuName = "Deposit/DepositSettings", order = 0)]
    public class DepositSettings : ScriptableObject
    {
        [field: SerializeField] public int MaxMining { get; private set; } = 3;
        [field: SerializeField] public float MiningCooldown { get; private set; } = 2f;
        [field: SerializeField] public Loot LootDropPerMine { get; private set; }
        [field: SerializeField] public float RestorationTime { get; private set; } = 5f;
    }
}