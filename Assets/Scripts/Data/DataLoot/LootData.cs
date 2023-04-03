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

        public void Collect(Loot loot)
        {
            if (_loot.ContainsKey(loot.Type))
            {
                _loot[loot.Type] += loot;
            }
            else
            {
                _loot[loot.Type] = loot;
            }

            Collected?.Invoke(new LootUpdatedArgs(loot.Type, _loot[loot.Type].Amount));
        }

        public Loot TrySubtract(LootType type, int desiredAmount)
        {
            if (!_loot.ContainsKey(type))
            {
                return null;
            }
            
            Loot loot = _loot[type].PickOutUpToX(desiredAmount);
            
            Subtracted?.Invoke(new LootUpdatedArgs(loot.Type, _loot[loot.Type].Amount));
            return loot;
        }

        // public bool HasDesiredLoot(LootType type, int amount) =>
            // _loot.ContainsKey(type) && _loot[type].Amount >= amount;

        public IEnumerable<Loot> GetAllLoot() => _loot.Values;
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