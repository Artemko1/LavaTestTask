using Data.DataLoot;
using UnityEngine;
using UnityEngine.Assertions;

namespace Logic.Spots
{
    public class Spot : MonoBehaviour
    {
        [SerializeField] private Loot _totalRequiredLoot;

        [SerializeField] private Transform _lootAcceptancePoint;

        public Loot RemainingRequiredLoot { get; private set; }

        public Vector3 LootAcceptancePoint => _lootAcceptancePoint.position;

        private void Awake()
        {
            Assert.IsFalse(_totalRequiredLoot.Type == LootType.None);
            Assert.IsFalse(_totalRequiredLoot.Amount <= 0);
            RemainingRequiredLoot = _totalRequiredLoot;
        }

        public void Collect(Loot loot)
        {
            Assert.AreEqual(_totalRequiredLoot.Type, loot.Type, "Trying to collect wrong loot");
            Assert.IsFalse(loot.Amount > RemainingRequiredLoot.Amount, "Too many loot is passed to the spot");

            int transferAmount = loot.Amount;
            RemainingRequiredLoot.Amount -= transferAmount;
            loot.Amount -= transferAmount;
        }
    }
}