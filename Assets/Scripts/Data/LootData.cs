using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class LootData
    {
        public Dictionary<LootType, int> Loot = new Dictionary<LootType, int>();

        public event Action<LootCollectedArgs> Collected;

        public void Collect(Loot loot)
        {
            if (Loot.ContainsKey(loot.Type))
            {
                Loot[loot.Type] += loot.Amount;
            }
            else
            {
                Loot[loot.Type] = loot.Amount;
            }

            Collected?.Invoke(new LootCollectedArgs(loot.Type, Loot[loot.Type]));
        }
    }

    public class LootCollectedArgs : EventArgs
    {
        public readonly int TotalAmount;
        public readonly LootType Type;

        public LootCollectedArgs(LootType type, int totalAmount)
        {
            Type = type;
            TotalAmount = totalAmount;
        }
    }

    public enum LootType
    {
        None = 0,
        Crystal = 1,
        Wood = 2,
        Metal = 3,
    }

    [Serializable]
    public class Loot
    {
        public int Amount;
        public LootType Type;
    }
}