using System;
using Data;
using Data.DataLoot;
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
}