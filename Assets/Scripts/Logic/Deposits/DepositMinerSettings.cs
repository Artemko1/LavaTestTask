using UnityEngine;

namespace Logic.Deposits
{
    [CreateAssetMenu(fileName = "DepositMinerSettings", menuName = "Deposit/DepositMinerSettings", order = 0)]
    public class DepositMinerSettings : ScriptableObject
    {
        [field: SerializeField] public float MiningRadius { get; private set; } = 1f;
    }
}