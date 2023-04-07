using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.DataLoot
{
    [Serializable]
    public class LootData
    {
        [JsonProperty] private Dictionary<LootType, Loot> _loot = new Dictionary<LootType, Loot>();

        public event Action<LootUpdatedArgs> Collected;
        public event Action<LootUpdatedArgs> Subtracted;

        public IEnumerable<Loot> GetAllLoot() => _loot.Values;

        public void Collect(Loot loot)
        {
            if (_loot.ContainsKey(loot.Type))
            {
                _loot[loot.Type].Amount += loot.Amount;
            }
            else
            {
                _loot[loot.Type] = loot.Clone();
            }

            Collected?.Invoke(new LootUpdatedArgs(loot.Type, _loot[loot.Type].Amount));
        }

        public Loot TrySubtractOne(LootType type)
        {
            if (!_loot.ContainsKey(type) || _loot[type].Amount <= 0)
            {
                return null;
            }

            Loot loot = _loot[type].PickOutUpToX(1);

            Subtracted?.Invoke(new LootUpdatedArgs(loot.Type, _loot[loot.Type].Amount));
            return loot;
        }
    }

    public class LootUpdatedArgs : EventArgs
    {
        public readonly int TotalAmount;
        public readonly LootType Type;

        public LootUpdatedArgs(LootType type, int totalAmount)
        {
            Type = type;
            TotalAmount = totalAmount;
        }
    }
}