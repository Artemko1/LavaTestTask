using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.DataLoot
{
    [Serializable]
    public class Loot
    {
        [SerializeField, JsonProperty]
        private LootType _type;

        [SerializeField, JsonProperty]
        private int _amount;

        public Loot(LootType type, int amount)
        {
            _type = type;
            Amount = amount;
        }

        public LootType Type => _type;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
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