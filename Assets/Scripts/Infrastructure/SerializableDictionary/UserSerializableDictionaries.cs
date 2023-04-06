using System;
using Data;
using Data.DataLoot;
using Logic.Drop;
using UnityEngine;

namespace Infrastructure.SerializableDictionary
{
    [Serializable]
    public class StringStringDictionary : SerializableDictionary<string, string>
    {
    }

    [Serializable]
    public class LootTypeSpriteDictionary : SerializableDictionary<LootType, Sprite>
    {
    }
    [Serializable]
    public class LootTypeDroppedLootDictionary : SerializableDictionary<LootType, DroppedLoot>
    {
    }
}