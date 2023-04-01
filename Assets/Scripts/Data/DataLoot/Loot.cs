using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Data.DataLoot
{
    [Serializable]
    public class Loot
    {
        public LootType Type;
        public int Amount;

        public Loot(LootType type, int amount)
        {
            Type = type;
            Amount = amount;
        }

        public static Loot operator +(Loot a, Loot b)
        {
            var loot = new Loot(a.Type, a.Amount + b.Amount);
            return loot;
        }

        [NotNull]
        public Loot PickOutUpToX(int x)
        {
            int pickOutAmount = Mathf.Min(Amount, x);
            Amount -= pickOutAmount;
            return new Loot(Type, pickOutAmount);
        }
    }
}